using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class InsurancePolicy
    {
        [Key]
        public int PolicyID { get; set; }

        [Required]
        [StringLength(100)]
        public string PolicyName { get; set; }

        public string Description { get; set; } // Gemini ile doldurulacak detaylar

        public decimal Price { get; set; } 

        public string ImageUrl { get; set; } 

        // Foreign Key (Kategori ile Bağlantı)
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual InsuranceCategory InsuranceCategory { get; set; }

        // İlişki: Bir poliçe binlerce kez satılabilir
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
