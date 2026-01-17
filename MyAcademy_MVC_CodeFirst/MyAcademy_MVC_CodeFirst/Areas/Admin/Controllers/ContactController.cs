using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.Filters;
using Serilog;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class ContactController : Controller
    {
        AppDbContext db = new AppDbContext();


        [LogAction(ActionDescription = "Gelen Mesaj Kutusu Görüntülendi")]
        public ActionResult Index()
        {
            var values = db.ContactMessages.OrderByDescending(x => x.SentDate).ToList();
            return View(values);
        }

        public ActionResult DeleteMessage(int id)
        {
            var value = db.ContactMessages.Find(id);
            string sender = value.Name;

            db.ContactMessages.Remove(value);
            db.SaveChanges();

            Log.Warning("Kullanıcı: {UserEmail} | İşlem: DeleteMessage | Mesaj: {Message} | Detay: {Data}",
        Session["Email"],
        $"{sender} kişisinden gelen mesaj silindi.",
        Newtonsoft.Json.JsonConvert.SerializeObject(new { ID = id, From = sender }));

            return RedirectToAction("Index");
        }

        [HttpGet]
        [LogAction(ActionDescription = "Bir Müşteri Mesajı Okundu")]
        public ActionResult MessageDetails(int id)
        {
            var value = db.ContactMessages.Find(id);

            if (!value.IsRead)
            {
                value.IsRead = true;
                db.SaveChanges();
            }

            return View(value);
        }
    }
}
