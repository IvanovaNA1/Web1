using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Delivery
    {
        [Key]public int DeliveryID { get; set; }
        public decimal DeliveryPrice { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
