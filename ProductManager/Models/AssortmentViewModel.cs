namespace ProductManager.MVC.Models
{
    public class AssortmentViewModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public List<AssortmentProductViewModel> AssortmentProducts { get; set; } = new();
    }
}
