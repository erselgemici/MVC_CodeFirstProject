using System;
using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        AppDbContext db = new AppDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            // Tüm poliçeleri listele
            var policies = db.InsurancePolicies.ToList();
            return View(policies);
        }

        [HttpPost]
        public ActionResult MakeRequest(int id)
        {
            var policy = db.InsurancePolicies.Find(id);
            var email = User.Identity.Name;
            var user = db.AppUsers.FirstOrDefault(x => x.Email == email);

            // Teklif isteğini "İletişim Mesajı" olarak kaydedelim (Pratik Çözüm)
            ContactMessage msg = new ContactMessage();
            msg.Name = user.FullName;
            msg.Email = user.Email;
            msg.Subject = "Teklif İsteği: " + policy.PolicyName;
            msg.MessageBody = $"Merhaba, {policy.PolicyName} ürünüyle ilgileniyorum. Lütfen bana dönüş yapın.";
            msg.SentDate = DateTime.Now;
            msg.IsRead = false;
            msg.AiCategory = "Satış Fırsatı"; // Admin panelinde dikkat çeksin

            db.ContactMessages.Add(msg);
            db.SaveChanges();

            // Başarılı mesajıyla geri dön
            TempData["Success"] = "Teklif talebiniz alındı! Uzmanlarımız sizi arayacak.";
            return RedirectToAction("Index");
        }
    }
}
