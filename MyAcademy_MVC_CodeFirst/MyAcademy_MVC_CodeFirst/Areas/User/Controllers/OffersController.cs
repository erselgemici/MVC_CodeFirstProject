using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    [LogAction(ActionDescription = "Kullanıcı Teklif İnceledi")]
    public class OffersController : Controller
    {
        AppDbContext db = new AppDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var policies = db.InsurancePolicies.ToList();
            return View(policies);
        }

        [HttpPost]
        public ActionResult MakeRequest(int id)
        {
            var policy = db.InsurancePolicies.Find(id);
            var email = User.Identity.Name;
            var user = db.AppUsers.FirstOrDefault(x => x.Email == email);

            ContactMessage msg = new ContactMessage();
            msg.Name = user.FullName;
            msg.Email = user.Email;
            msg.Subject = "Teklif İsteği: " + policy.PolicyName;
            msg.MessageBody = $"Merhaba, {policy.PolicyName} ürünüyle ilgileniyorum. Lütfen bana dönüş yapın.";
            msg.SentDate = DateTime.Now;
            msg.IsRead = false;
            msg.AiCategory = "Satış Fırsatı"; 

            db.ContactMessages.Add(msg);
            db.SaveChanges();

            TempData["Success"] = "Teklif talebiniz alındı! Uzmanlarımız sizi arayacak.";
            return RedirectToAction("Index");
        }
    }
}
