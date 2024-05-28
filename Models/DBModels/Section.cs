using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Section
    {
        [Key]public int SectionID { get; set; }
        public int IceboxID { get; set; }
    }
}
