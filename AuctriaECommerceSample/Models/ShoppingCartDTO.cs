namespace AuctriaECommerceSample.Models
{
    /// <summary>
    /// Aggregate Item and ShoppingCart
    /// </summary>
    public class ShoppingCartDTO
    {
        public int Id { get; set; }
        public int RowNo { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
