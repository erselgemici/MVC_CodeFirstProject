using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class AppUser
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Ad Soyad boş geçilemez")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
