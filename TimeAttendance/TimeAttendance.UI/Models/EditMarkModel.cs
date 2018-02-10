using System;
using System.ComponentModel.DataAnnotations;

namespace TimeAttendance.UI.Models
{
    public class EditMark
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public DateTime Coming_Date { get; set; }
        
        public DateTime? Out_Date { get; set; }

        public string Author { get; set; }

        public string UserName { get; set; }
    }
}