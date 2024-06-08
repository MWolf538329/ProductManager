namespace ProductManager.Core.Models
{
    public class AssortmentProduct
    {
        public int Id { get; set; }
        public int BranchID { get; set; } = new();
        public Product Product { get; set; } = new();
        public int Stock { get; set; }
    }
}
