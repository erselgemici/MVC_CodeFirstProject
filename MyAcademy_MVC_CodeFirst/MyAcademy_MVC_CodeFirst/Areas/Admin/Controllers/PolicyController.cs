using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class PolicyController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var values = db.InsurancePolicies.Include("InsuranceCategory").ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult CreatePolicy()
        {
            List<SelectListItem> categories = (from x in db.InsuranceCategories.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.CategoryID.ToString()
                                               }).ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public ActionResult CreatePolicy(InsurancePolicy policy)
        {
            if (ModelState.IsValid)
            {
                db.InsurancePolicies.Add(policy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return CreatePolicy();
        }

        public ActionResult DeletePolicy(int id)
        {
            var value = db.InsurancePolicies.Find(id);
            db.InsurancePolicies.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdatePolicy(int id)
        {
            List<SelectListItem> categories = (from x in db.InsuranceCategories.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.CategoryID.ToString()
                                               }).ToList();
            ViewBag.Categories = categories;

            var value = db.InsurancePolicies.Find(id);
            return View(value);
        }

        [HttpPost]
        public ActionResult UpdatePolicy(InsurancePolicy policy)
        {
            var value = db.InsurancePolicies.Find(policy.PolicyID);
            value.PolicyName = policy.PolicyName;
            value.Description = policy.Description;
            value.Price = policy.Price;
            value.ImageUrl = policy.ImageUrl;
            value.CategoryID = policy.CategoryID;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
