namespace ProductManager.Core.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public Category Category { get; set; } = new Category();
        public decimal Price { get; set; }
        public int Contents { get; set; }
        public Unit Unit { get; set; }
    }
}
