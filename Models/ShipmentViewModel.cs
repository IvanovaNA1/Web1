using Web1.Models.DBModels;

namespace Web1.Models
{
    public class ShipmentViewModel
    {
        public int ShipmentID { get; set; }
        public string ProviderName { get; set; }
        public string WarehouseName { get; set; }
        public DateTime ShipmentDate { get; set; }
        public List<ShipmentList> ShipmentLists { get; set; }
    }
}
