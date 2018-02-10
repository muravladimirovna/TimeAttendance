using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeAttendance.UI.Models
{
    public class EditViewModel
    {
        [Required]
        public int id { get; set; }
        //[Required]
        public string username { get; set; }
        [Required]
        public string firstname { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        public string middlename { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string phonenumber { get; set; }
        //[Required]
        public string addrole { get; set; }
        //[Required]
        public string delrole { get; set; }
    }
}