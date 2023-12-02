using AuctriaECommerceSample.Models;
using AuctriaECommerceSample.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AuctriaECommerceSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : Controller
    {
        [HttpPost]
        public IActionResult UpdateShoopingCart([FromBody]ShoppingCart shoppingCart)
        {
            if (!SharedVariables.shoppingCartManager.UpdateShoppingCart(shoppingCart))
                return BadRequest();
            return Ok(shoppingCart);
        }

        [HttpGet("ListShoppingCartContents")]
        public IActionResult GetShoppingCartContents()
        {
            return Ok(SharedVariables.shoppingCartManager.GetShoppingCartContents());
        }
    }
}
