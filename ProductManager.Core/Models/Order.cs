namespace ProductManager.Core.Models
{
    public class Order
    {
        public int ID { get; set; }
        public Branch Branch { get; set; } = new();
        public Customer Customer { get; set; } = new();
        public List<OrderLine> OrderLines { get; set; } = new();
    }
}
