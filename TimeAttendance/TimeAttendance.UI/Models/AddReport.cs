using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeAttendance.UI.Models
{
    public class AddReport
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string MiddleName { get; set; }

        public DateTime Date { get; set; }

    }
}