using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendance.Domain.Models
{
    [Table("mark", Schema = "ta")]
    public class Marks
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("coming_date")]
        public DateTime Coming_Date { get; set; }

        [Column("out_date")]
        public DateTime? Out_Date { get; set; }

        [Column("AuthorId")]
        public int AuthorId { get; set; }

        //[ForeignKey("UserId")]
        //public virtual AppUser User { get; set; }

        //[ForeignKey("AuthorId")]
        //public virtual AppUser Author { get; set; }

    }
}
