using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Testimonial
    {
        [Key]
        public int TestimonialID { get; set; }

        public string ClientName { get; set; }
        public string Profession { get; set; } 
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
    }
}
