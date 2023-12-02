using System.Data;
using AuctriaECommerceSample.Models;
using AuctriaECommerceSample.Utils;

namespace AuctriaECommerceSample.Managers
{
    /// <summary>
    /// manage CRUD operations of item list
    /// </summary>
    public class ShoppingCartManager
    {
        /// <summary>
        /// check the cart items. like id and quantity
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        /// <exception cref="CustomValidationException"></exception>
        private bool CanUpdateShoppingCart(ShoppingCart cart)
        {
            if (cart == null)
                throw new CustomValidationException("Shopping cart is not specified");

            List<string> lstValidationErrors = new List<string>();
            #region check the existance of item
            var item = SharedVariables.ItemManager.RetrieveItem(cart.Id);
            if (item == null)
                lstValidationErrors.Add($"There is no item with id = {cart.Id} in the list of Items");

            #endregion

            #region checkPrice
            if (cart.Quantity <= 0)
                lstValidationErrors.Add($"Quantity ('{cart.Quantity}') is not a positive value");
            #endregion

            if (lstValidationErrors.Count > 0)
                throw new CustomValidationException(lstValidationErrors);
            return true;
        }

        public bool UpdateShoppingCart(ShoppingCart cart)
        {
            if (!CanUpdateShoppingCart(cart))
                return false;
            var shoppingCartItem = SharedVariables.ShoppingCart.FirstOrDefault(o => o.Id == cart.Id);
            if (shoppingCartItem == null)
            {
                SharedVariables.ShoppingCart.Add(cart);
            }
            else
            {
                shoppingCartItem.Quantity = cart.Quantity;
            }
            return true;
        }

        public List<ShoppingCartDTO> GetShoppingCartContents()
        {
            List<ShoppingCartDTO> lstShoppingDTO = SharedVariables.ShoppingCart
                                                .Join(SharedVariables.Items,
                                                sc => sc.Id,
                                                i => i.Id,
                                                (sc, i) => new { sc, i })
                                                .OrderBy(o => o.i.Title)
                                                .ThenBy(o => o.i.Price)
                                                .ThenBy(o => o.sc.Quantity)
                                                .Select((item, index) => new ShoppingCartDTO
                                                {
                                                    RowNo = index + 1,
                                                    Id = item.sc.Id,
                                                    Title = item.i.Title,
                                                    Price = item.i.Price,
                                                    Quantity = item.sc.Quantity,
                                                    TotalPrice = item.i.Price * item.sc.Quantity
                                                })
                                                .ToList();
            //last item is the total of the shopping card regarding Quantity and TotalPrice
            lstShoppingDTO.Add(new ShoppingCartDTO
            {
                RowNo = -1,
                Id = -1,
                Title = "Total",
                Price = 0,
                Quantity = lstShoppingDTO.Sum(o => o.Quantity),
                TotalPrice = lstShoppingDTO.Sum(o => o.TotalPrice)
            });
            return lstShoppingDTO;
        }
    }
}
