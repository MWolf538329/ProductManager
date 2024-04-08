using ProductManager.Classes;

namespace ProductManager.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Contents { get; set; }
        public Data.Enum.Unit Unit { get; set; }
        public string Description { get; set; }
    }
}
