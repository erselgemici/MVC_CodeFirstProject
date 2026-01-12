using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class TeamMember
    {
        [Key]
        public int TeamMemberID { get; set; }

        public string FullName { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string LinkedInUrl { get; set; }
    }
}
