namespace ProductManager.MVC.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public List<OrderLineViewModel> OrderLines { get; set; } = new();
    }
}
