using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAttendance.UI.Models
{
    public class ReportInfo
    {
        public int Task { get; set; } // норма часов/мес

        public List<DateTime> Days { get; set; } // выходные дни  
         
        public List<Days> All { get; set; } // кол-во рабочих часов по датам

        public int Count { get; set; } // кол-во рабочих дней

        public string MName { get; set; } // название месяца
    }
}