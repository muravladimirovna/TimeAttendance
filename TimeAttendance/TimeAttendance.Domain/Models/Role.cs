using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendance.Domain.Models
{
    [Table("role", Schema ="ta")]
    public class Role: IdentityRole<int, UserRole>
    {
        //[Column("id")]
        //public int Id { get; set; }

        //[Column("name")]
        //public string Name { get; set; }
    }
}
