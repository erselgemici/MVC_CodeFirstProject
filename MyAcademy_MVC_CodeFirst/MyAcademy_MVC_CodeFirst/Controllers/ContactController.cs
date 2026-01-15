using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
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
        public async Task<ActionResult> Index(ContactMessage message)
        {
            var settings = db.CompanySettings.FirstOrDefault();
            ViewBag.Settings = settings;

            message.SentDate = DateTime.Now;
            message.IsRead = false;

            try
            {
                message.AiCategory = await aiService.GetCategoryWithHuggingFace(message.MessageBody);
            }
            catch { message.AiCategory = "Genel"; }

            var aiResponse = await aiService.GenerateReplyWithChatGPT(message.MessageBody, message.AiCategory);

            message.AiResponse = aiResponse.EmailReply;

            db.ContactMessages.Add(message);
            db.SaveChanges();

            try
            {
                await emailService.SendEmailAsync(message.Email, $"Talebini Alındı: {message.AiCategory}", message.AiResponse);

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
