namespace ProductManager.Models
{
    public class ProductOverviewViewModel
    {
        public IReadOnlyList<Classes.ProductBase> Products { get { return _products; } }
        private List<Classes.ProductBase> _products { get; set; }
    }
}
