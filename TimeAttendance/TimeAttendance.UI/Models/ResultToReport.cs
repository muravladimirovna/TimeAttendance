using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TimeAttendance.Domain.Models;

namespace TimeAttendance.UI.Models
{
    public class ResultToReport
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string middlename { get; set; }

        [Required]
        public int sum { get; set; } // всего часов

        public int dayall { get; set; } // отработанные дни

        public int dayoff { get; set; } // отработал в выходные

        public int no { get; set; } // прогулы

        public int lateness { get; set; } // опоздания

        public List<DateTime> latelist { get; set; } // опоздания

        public int early { get; set; } // опоздания

        public List<DateTime> earlylist { get; set; } // опоздания

        [ForeignKey("UserId")]
        public virtual List<Days> Days { get; set; } // кол-во часов по датам
    }
}