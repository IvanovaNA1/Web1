using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Shipment
    {
        [Key]
        public int ShipmentID { get; set; }
        public int ProviderID { get; set; }
        public int WarehouseID { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
