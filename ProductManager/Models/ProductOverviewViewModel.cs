namespace ProductManager.Models
{
    public class ProductOverviewViewModel
    {
        public IReadOnlyList<Classes.BaseProduct> Products { get { return _products; } }
        private List<Classes.BaseProduct> _products { get; set; }
    }
}
