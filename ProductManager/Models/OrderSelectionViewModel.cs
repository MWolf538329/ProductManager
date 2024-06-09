namespace ProductManager.MVC.Models
{
    public class OrderSelectionViewModel
    {
        public List<CustomerViewModel> customers { get; set; } = new();
        public List<BranchViewModel> branches { get; set; } = new();
    }
}
