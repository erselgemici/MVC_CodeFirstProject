using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        AppDbContext db = new AppDbContext();

        // HASARLARIM LİSTESİ
        public ActionResult Index()
        {
            var email = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(x => x.Email == email);
            if (customer == null) return RedirectToAction("Index", "Dashboard");

            var myClaims = db.Claims.Where(x => x.CustomerID == customer.CustomerID).OrderByDescending(x => x.CreatedAt).ToList();
            return View(myClaims);
        }

        // YENİ HASAR KAYDI (GET)
        [HttpGet]
        public ActionResult NewClaim()
        {
            var email = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(x => x.Email == email);

            // Müşterinin SAHİP OLDUĞU poliçeleri listele (Sadece onlara hasar açabilir)
            // Sales tablosundan müşterinin poliçelerini bulup distinct (tekrarsız) getiriyoruz
            var myPolicyIds = db.Sales.Where(x => x.CustomerID == customer.CustomerID).Select(x => x.PolicyID).Distinct().ToList();

            var myPolicies = db.InsurancePolicies
                               .Where(x => myPolicyIds.Contains(x.PolicyID))
                               .Select(x => new SelectListItem
                               {
                                   Text = x.PolicyName,
                                   Value = x.PolicyID.ToString()
                               }).ToList();

            ViewBag.Policies = myPolicies;
            return View();
        }

        // YENİ HASAR KAYDI (POST)
        [HttpPost]
        public ActionResult NewClaim(Claim p)
        {
            var email = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(x => x.Email == email);

            p.CustomerID = customer.CustomerID;
            p.CreatedAt = DateTime.Now;
            p.Status = "Beklemede"; // İlk kayıt durumu

            db.Claims.Add(p);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
