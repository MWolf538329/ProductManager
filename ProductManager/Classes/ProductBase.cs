namespace ProductManager.Classes
{
    public class ProductBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Contents { get; set; }
        public string Unit { get; set; }
    }
}
