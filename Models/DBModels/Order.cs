using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int DeliveryID { get; set; }
        public DateTime OrderDate { get; set; }
        public Delivery Delivery { get; set; }
        public User User { get; set; }
        //public List<OrderProducts> OrderProducts { get; set; }
        //public List<OrderServices> OrderServices { get; set; }
    }
}
