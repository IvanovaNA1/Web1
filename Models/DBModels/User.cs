using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class User
    {
        [Key] public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime UserBirthday { get; set; }
        public string UserPhone { get; set; }
        public string UserAccount { get; set; }
    }
}
