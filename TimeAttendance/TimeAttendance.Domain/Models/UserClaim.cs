using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendance.Domain.Models
{
    [Table("user_claim", Schema = "ta")]
    public class UserClaim: IdentityUserClaim<int>
    {
        //[Key]
        //[Column("Id")]
        //public int Id { get; set; }

        //[Column("UserId")]
        //public int UserId { get; set; }

        //[Column("ClaimType")]
        //public string ClaimType { get; set; }

        //[Column("ClaimValue")]
        //public string ClaimValue { get; set; }
    }
}