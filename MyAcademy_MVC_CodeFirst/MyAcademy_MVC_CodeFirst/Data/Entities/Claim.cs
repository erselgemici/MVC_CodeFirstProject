using System;
using System.ComponentModel.DataAnnotations;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Claim
    {
        [Key]
        public int ClaimID { get; set; }
        public int CustomerID { get; set; }
        public int PolicyID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // "Beklemede", "İşlemde", "Onaylandı"
        public DateTime IncidentDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual InsurancePolicy InsurancePolicy { get; set; }
    }
}
