namespace ProductManager.MVC.Models
{
    public class OrderLineViewModel
    {
        public int Id { get; set; }
        public ProductViewModel Product { get; set; } = new();
        public int Amount { get; set; }
    }
}
