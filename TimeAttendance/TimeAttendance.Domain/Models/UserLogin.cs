using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimeAttendance.Domain.Models
{
    [Table("user_login", Schema = "ta")]
    public class UserLogin: IdentityUserLogin<int>
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        //[Key]
        //[Column("UserId")]
        //public int UserId { get; set; }

        //[Column("LoginProvider")]
        //public string LoginProvider { get; set; }

        //[Column("ProviderKey")]
        //public string ProviderKey { get; set; }
    }
}
