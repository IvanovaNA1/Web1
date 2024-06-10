using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Warehouse
    {
        [Key]public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseAddress { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
    }
}
