using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class Category
    {
        [Key]public string CategoryName { get; set; }
        public override string ToString()
        {
            return CategoryName;
        }
    }
}
