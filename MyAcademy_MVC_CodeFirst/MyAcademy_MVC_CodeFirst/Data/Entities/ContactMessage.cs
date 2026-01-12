using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class ContactMessage
    {
        [Key]
        public int MessageID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public DateTime SentDate { get; set; }

        public bool IsRead { get; set; }
        public string AiCategory { get; set; } // Hugging Face (Teşekkür, Şikayet vb.)
        public string AiResponse { get; set; } // ChatGPT'nin ürettiği cevap
    }
}
