using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models.DBModels
{
    public class UserAccount
    {
        [Key]public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public int UserRole { get; set; }
    }
}
