using System.Reflection.Metadata.Ecma335;

namespace ProductManager.MVC.Models
{
    public class OrderCreationViewModel
    {
        public int CustomerID { get; set; }
        public int BranchID { get; set; }
        List<OrderLineViewModel> orderLines { get; set; } = new();
        //List<ProductViewModel>
    }
}
