using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Provider
    {
        [Key]public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string ProviderEmail { get; set; }
    }
}
