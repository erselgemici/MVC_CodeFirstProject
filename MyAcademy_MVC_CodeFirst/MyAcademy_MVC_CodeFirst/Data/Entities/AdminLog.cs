using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class AdminLog
    {
        [Key]
        public int LogID { get; set; }
        public string ActionType { get; set; } // Create, Update, Delete
        public string Description { get; set; }
        public DateTime LogDate { get; set; }
        public string IpAddress { get; set; }
    }
}
