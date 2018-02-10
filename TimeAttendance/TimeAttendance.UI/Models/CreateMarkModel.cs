using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeAttendance.Domain.Models;

namespace TimeAttendance.UI.Models
{
    public class CreateMarkModel
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public bool warning { get; set; }

        public DateTime ServerDate { get; set; }

    }
}