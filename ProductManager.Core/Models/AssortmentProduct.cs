namespace ProductManager.Core.Models
{
    public class AssortmentProduct
    {
        public int Id { get; set; }
        public Product Product { get; set; } = new();
        public int Stock { get; set; }
    }
}
