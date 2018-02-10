using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendance.Domain.Models
{
    [Table("user_role", Schema = "ta")]
    public class UserRole: IdentityUserRole<int>
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        //[Key]
        //[Column("UserId")]
        //public int UserId { get; set; }

        //[Column("RoleId")]
        //public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
