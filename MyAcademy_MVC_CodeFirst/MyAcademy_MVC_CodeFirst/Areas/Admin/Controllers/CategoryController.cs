using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var values = db.InsuranceCategories.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(InsuranceCategory category)
        {
            if (ModelState.IsValid)
            {
                db.InsuranceCategories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult DeleteCategory(int id)
        {
            var category = db.InsuranceCategories.Find(id);
            db.InsuranceCategories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            var category = db.InsuranceCategories.Find(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult UpdateCategory(InsuranceCategory category)
        {
            var value = db.InsuranceCategories.Find(category.CategoryID);
            value.CategoryName = category.CategoryName;
            value.IconClass = category.IconClass;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
