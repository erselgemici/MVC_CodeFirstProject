using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal Amount { get; set; } 

        // Foreign Key (Poliçe ile Bağlantı)
        public int PolicyID { get; set; }
        [ForeignKey("PolicyID")]
        public virtual InsurancePolicy InsurancePolicy { get; set; }

        // Foreign Key (Müşteri ile Bağlantı)
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
}
