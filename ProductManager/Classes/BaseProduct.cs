namespace ProductManager.Classes
{
    public class BaseProduct
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Contents { get; set; }
        public Unit Unit { get; set; }
    }
}
