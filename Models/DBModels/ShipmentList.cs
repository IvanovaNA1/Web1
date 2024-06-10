using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class ShipmentList
    {
        [Key]
        public int ShipmentListId { get; set; }
        public int ShipmentID { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public Shipment Shipment { get; set; }

    }
}
