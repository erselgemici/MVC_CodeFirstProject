using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class FeatureController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var values = db.Features.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult CreateFeature()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFeature(Feature feature)
        {
            db.Features.Add(feature);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFeature(int id)
        {
            var value = db.Features.Find(id);
            db.Features.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateFeature(int id)
        {
            var value = db.Features.Find(id);
            return View(value);
        }

        [HttpPost]
        public ActionResult UpdateFeature(Feature feature)
        {
            var value = db.Features.Find(feature.FeatureID);
            value.Title = feature.Title;
            value.Description = feature.Description;
            value.IconClass = feature.IconClass;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
