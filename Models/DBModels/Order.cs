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
    }
}
