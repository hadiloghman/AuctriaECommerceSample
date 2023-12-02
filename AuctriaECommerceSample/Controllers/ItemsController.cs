using AuctriaECommerceSample.Models;
using AuctriaECommerceSample.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.ComponentModel;

namespace AuctriaECommerceSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        
        [HttpPost("AddItem")]
        public IActionResult AddItem([FromBody] Item item)
        {
            //item.id is set to null for detecting the correct operation in SaveItem
            if (item != null)
                item.Id = null;
            SharedVariables.ItemManager.SaveItem(item);

            return CreatedAtAction(nameof(RetrieveItem), new { id = item.Id }, item.Id);
        }

        [HttpPost("UpdateItem")]
        public IActionResult UpdateItem([FromBody] Item item)
        {
            SharedVariables.ItemManager.SaveItem(item);

            return CreatedAtAction(nameof(RetrieveItem), new { id = item.Id }, item.Id);
        }

        [HttpGet("RetrieveItem/{id}")]
        public IActionResult RetrieveItem(int id)
        {
            var item = SharedVariables.ItemManager.RetrieveItem(id);
            if (item == null)
                return NotFound(id);
            return Ok(item);
        }

        [HttpGet("RetrieveItemByTitle/{title}")]
        public IActionResult RetrieveItemByTitle(string title)
        {
            var item = SharedVariables.ItemManager.RetrieveItem(title);
            if (item == null)
                return NotFound(title);
            return Ok(item);
        }

        [HttpGet("DeleteItem/{id}")]
        public IActionResult DeleteItem(int id)
        {
            SharedVariables.ItemManager.DeleteItem(id);
            return Ok(id);
        }

        [HttpGet("DeleteItemByTitle/{title}")]
        public IActionResult DeleteItemByTitle(string title)
        {
            SharedVariables.ItemManager.DeleteItem(title);
            return Ok(title);
        }

        [HttpGet("ListAllItems")]
        public IActionResult ListAllItems()
        {
            return Ok(SharedVariables.Items);
        }

    }
}
