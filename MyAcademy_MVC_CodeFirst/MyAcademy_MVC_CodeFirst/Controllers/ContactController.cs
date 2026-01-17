using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities; 
using MyAcademy_MVC_CodeFirst.Filters; 
using MyAcademy_MVC_CodeFirst.Services; 
using System;
using System.Linq;
using System.Threading.Tasks; 
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Controllers
{
    public class ContactController : Controller
    {
        AppDbContext db = new AppDbContext();

        AiService aiService = new AiService();
        EmailService emailService = new EmailService();

        [HttpGet]
        public ActionResult Index()
        {
            var settings = db.CompanySettings.FirstOrDefault();

            ViewBag.Settings = settings;

            return View();
        }

        [HttpPost]
        [LogAction(ActionDescription = "İletişim Formundan Mesaj Gönderildi")]
        public async Task<ActionResult> Index(ContactMessage message)
        {
            var settings = db.CompanySettings.FirstOrDefault();
            ViewBag.Settings = settings;

            message.SentDate = DateTime.Now; 
            message.IsRead = false;          

            // HuggingFace modelini kullanarak mesajın konusunu (Şikayet, Teşekkür, Satış vb.) tahmin ettiriyoruz.
            try
            {
                message.AiCategory = await aiService.GetCategoryWithHuggingFace(message.MessageBody);
            }
            catch
            {
                message.AiCategory = "Genel";
            }

            // ChatGPT'ye "Müşteri böyle dedi, kategorisi de bu, ona uygun kibar bir cevap yaz" diyoruz.
            var aiResponse = await aiService.GenerateReplyWithChatGPT(message.MessageBody, message.AiCategory);

            message.AiResponse = aiResponse.EmailReply;

            db.ContactMessages.Add(message);
            db.SaveChanges(); 

            // Müşteriye "Mesajınız alındı" demek yerine, ChatGPT'nin hazırladığı cevabı mail atıyoruz.
            try
            {
                await emailService.SendEmailAsync(message.Email, $"Talebini Alındı: {message.AiCategory}", message.AiResponse);

                // ChatGPT sadece mail yazmadı, ekranda çıkacak "Teşekkürler" mesajını da hazırladı (UiTitle, UiText).
                // Bunu ViewBag ile ekrana basıyoruz ki kullanıcı "Robotla konuşuyor gibi" hissetmesin, samimi olsun.
                ViewBag.UiTitle = aiResponse.UiTitle;
                ViewBag.UiText = aiResponse.UiText;
                ViewBag.ButtonText = aiResponse.UiButton;

                ViewBag.Status = "Success"; 
            }
            catch
            {
                ViewBag.Status = "MailError";
            }

            return View();
        }
    }
}
