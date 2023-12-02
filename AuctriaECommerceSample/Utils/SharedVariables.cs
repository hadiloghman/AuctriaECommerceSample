using AuctriaECommerceSample.Managers;
using AuctriaECommerceSample.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuctriaECommerceSample.Utils
{
    //singleton class
    public class SharedVariables
    {
        private static List<ShoppingCart> _shoppingCart;
        public static List<ShoppingCart> ShoppingCart
        {
            get
            {
                if (_shoppingCart == null)
                    _shoppingCart = new List<ShoppingCart>();
                return _shoppingCart;
            }
        }

        private static List<Item> _items;
        public static List<Item> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<Item>();
                }
                return _items;
            }
        }

        private static ItemManager _itemManager;
        public static ItemManager ItemManager
        {
            get
            {
                if (_itemManager == null)
                {
                    _itemManager = new ItemManager();
                }
                return _itemManager;
            }
        }

        private static ShoppingCartManager _shoppingCartManager;
        public static ShoppingCartManager shoppingCartManager
        {
            get
            {
                if (_shoppingCartManager == null)
                {
                    _shoppingCartManager = new ShoppingCartManager();
                }
                return _shoppingCartManager;
            }
        }
    }
}
