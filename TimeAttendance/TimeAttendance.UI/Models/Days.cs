using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeAttendance.UI.Models
{
    public class Days
    {
        [Key]
        public int UserId { get; set; }

        public DateTime date { get; set; }

        public int? sum { get; set; }
    }
}