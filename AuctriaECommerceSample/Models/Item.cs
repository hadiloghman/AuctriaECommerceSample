using System.Net;

namespace AuctriaECommerceSample.Models
{
    public class Item
    {
        /// <summary>
        /// in add operation Id == null
        /// </summary>
        public int? Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

    }
}
