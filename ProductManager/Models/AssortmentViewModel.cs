namespace ProductManager.MVC.Models
{
    public class AssortmentViewModel
    {
        public string BranchName { get; set; } = string.Empty;
        public List<AssortmentProductViewModel> AssortmentProducts { get; set; } = new();
    }
}
