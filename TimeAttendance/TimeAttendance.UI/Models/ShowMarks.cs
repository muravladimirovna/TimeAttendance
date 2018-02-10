using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeAttendance.UI.Models
{
    public class ShowMarks
    {
        //[Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("coming_date")]
        public DateTime Coming_Date { get; set; }

        [Column("out_date")]
        public DateTime? Out_Date { get; set; }

        public string Author { get; set; }

        public string UserName { get; set; }
    }
}