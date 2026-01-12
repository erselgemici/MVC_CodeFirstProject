using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        // ML.Net tahmini için şehir
        public string City { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // İlişki: Bir müşterinin birden fazla satışı olabilir
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
