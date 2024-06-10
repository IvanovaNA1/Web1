using Web1.Models.DBModels;

namespace Web1.Models
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderProducts> OrderProductsList { get; set; }
        public List<OrderServices> OrderServicesList { get; set; }
    }
}
