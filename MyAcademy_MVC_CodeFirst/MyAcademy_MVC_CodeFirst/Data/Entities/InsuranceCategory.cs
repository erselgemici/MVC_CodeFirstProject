using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class InsuranceCategory
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } 

        public string IconClass { get; set; }

        // İlişki: Bir kategoride birden fazla poliçe/paket olabilir
        public virtual ICollection<InsurancePolicy> InsurancePolicies { get; set; }
    }
}
