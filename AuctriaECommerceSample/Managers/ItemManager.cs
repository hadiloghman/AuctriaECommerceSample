using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Security.Cryptography.Xml;
using AuctriaECommerceSample.Models;
using AuctriaECommerceSample.Utils;

namespace AuctriaECommerceSample.Managers
{
    /// <summary>
    /// manage CRUD operations of item list
    /// </summary>
    public class ItemManager
    {
        /// <summary>
        /// Can Create/Update item 
        /// </summary>
        /// <param name="item">if item.id ==null then the operation is 'create' otherwise 'update'</param>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="CustomValidationException"></exception>
        private bool CanSaveItem(Item item)
        {
            List<string> lstValidationErrors = new List<string>();

            if (item == null)
                throw new CustomValidationException("Item is not specified");


            if (item.Id.HasValue && RetrieveItem(item.Id.Value) == null)
            {
                //in edit mode item does not exist. log error
                lstValidationErrors.Add($"There is no item with Id = {item.Id} in the list");
            }

            #region check the title
            //trim start and end of title
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                lstValidationErrors.Add("Title of the item is not specified");
            }
            else
            {
                //check duplicate title for insert and update
                if (SharedVariables.Items.Any(o => o.Title == item.Title.Trim()
                                               && (
                                                    item.Id.HasValue && o.Id != item.Id//for edit
                                                    || !item.Id.HasValue//for insert
                                                   )
                                              )
                   )
                    lstValidationErrors.Add($"Title ('{item.Title}') is duplicated");

            }

            #endregion

            #region checkPrice
            if (item.Price <= 0)
                lstValidationErrors.Add($"Price ('{item.Price}') is not a positive value");
            #endregion

            if (lstValidationErrors.Count > 0)
                //if there is any error throw errors
                throw new CustomValidationException(lstValidationErrors);

            return true;
        }

        /// <summary>
        /// finditem by id and check if we can delete the item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomValidationException"></exception>
        private bool CanDeleteItem(int id)
        {
            var item = RetrieveItem(id);
            if (item == null)
                throw new CustomValidationException($"There is no item with Id = {id} in the list");


            return CanDeleteItem(item);
        }

        /// <summary>
        /// finditem by title and check if we can delete the item
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        /// <exception cref="CustomValidationException"></exception>
        private bool CanDeleteItem(string title)
        {
            var item = RetrieveItem(title);
            if (item == null)
                throw new CustomValidationException($"There is no item with title = '{title}' in the list");

            return CanDeleteItem(item);
        }

        /// <summary>
        /// check if we can delete the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="CustomValidationException"></exception>
        private bool CanDeleteItem(Item item)
        {
            List<CustomValidationException> lstValidation = new List<CustomValidationException>();
            if (item == null)
                throw new CustomValidationException($"Item is null");
            else if (SharedVariables.ShoppingCart.Any(o => o.Id == item.Id))
                throw new CustomValidationException($"Item '{item.Title}' is used in shopping cart and cannot be deleted");

            return true;
        }

        /// <summary>
        /// saveitem (add or update)
        /// </summary>
        /// <param name="item">if item.id == null then the operation is 'create' otherwise 'edit' </param>
        /// <returns></returns>
        public bool SaveItem(Item item)
        {
            if (!CanSaveItem(item))
                return false;

            //trim the spaces
            item.Title = item.Title.Trim();

            if (item.Id.HasValue)
            {
                //edit mode
                var itemMain = RetrieveItem(item.Id.Value);
                itemMain.Title = item.Title;
                itemMain.Price = item.Price;
            }
            else
            {
                //add mode
                item.Id = (SharedVariables.Items.Any() ? SharedVariables.Items.Max(o => o.Id) : 0) + 1;
                SharedVariables.Items.Add(item);
            }
            return true;

        }

        /// <summary>
        /// retrieveItem by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item RetrieveItem(int id)
        {
            return SharedVariables.Items.FirstOrDefault(o => o.Id == id);

        }

        /// <summary>
        /// retrieveItem by Title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Item RetrieveItem(string title)
        {
            return SharedVariables.Items.FirstOrDefault(o => o.Title == title);

        }

        /// <summary>
        /// DeleteItem by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteItem(int id)
        {
            if (!CanDeleteItem(id))
                return false;

            SharedVariables.Items.RemoveAll(o => o.Id == id);
            return true;

        }

        /// <summary>
        /// DeleteItem by Title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool DeleteItem(string title)
        {
            if (!CanDeleteItem(title))
                return false;

            SharedVariables.Items.RemoveAll(o => o.Title == title);
            return true;

        }
    }
}
