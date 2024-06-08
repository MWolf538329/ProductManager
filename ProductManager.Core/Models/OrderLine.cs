namespace ProductManager.Core.Models
{
    public class OrderLine
    {
        public int ID { get; set; }
        public Product Product { get; set; } = new();
        public int Amount { get; set; }
    }
}
