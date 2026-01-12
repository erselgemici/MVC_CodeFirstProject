using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Faq
    {
        [Key]
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
