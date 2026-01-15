using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Staff.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        AppDbContext db = new AppDbContext();

        // 1. SATIŞ GEÇMİŞİ LİSTESİ
        public ActionResult Index()
        {
            // Sadece son 50 satışı getir (Performans için)
            var sales = db.Sales.OrderByDescending(x => x.SaleID).Take(50).ToList();
            return View(sales);
        }

        // 2. YENİ SATIŞ SAYFASI (GET)
        [HttpGet]
        public ActionResult NewSale()
        {
            // Müşterileri Dropdown'a doldur
            List<SelectListItem> customers = (from x in db.Customers.OrderBy(c => c.FirstName).ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.FirstName + " " + x.LastName + " - " + x.City,
                                                  Value = x.CustomerID.ToString()
                                              }).ToList();

            // Poliçeleri Dropdown'a doldur
            List<SelectListItem> policies = (from x in db.InsurancePolicies.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.PolicyName + " (" + x.Price.ToString("C0") + ")",
                                                 Value = x.PolicyID.ToString()
                                             }).ToList();

            ViewBag.CList = customers;
            ViewBag.PList = policies;

            return View();
        }

        [HttpPost]
        public ActionResult NewSale(Sale p, string CustomerType, string NewFirstName, string NewLastName, string NewCity, string NewPhone, int Duration)
        {
            // 1. MÜŞTERİ KONTROLÜ
            if (CustomerType == "new")
            {
                // Yeni Müşteri Oluştur ve Kaydet
                Customer c = new Customer();
                c.FirstName = NewFirstName;
                c.LastName = NewLastName;
                c.City = NewCity;
                c.Phone = NewPhone;
                c.Email = NewFirstName.ToLower() + "." + NewLastName.ToLower() + "@mail.com"; // Basit mail üretme
                c.CreatedAt = DateTime.Now;

                db.Customers.Add(c);
                db.SaveChanges(); // ID oluşsun diye hemen kaydediyoruz

                p.CustomerID = c.CustomerID; // Yeni oluşan ID'yi satışa ata
            }
            // Else: CustomerType == "existing" ise p.CustomerID zaten formdan dolu geliyor.

            // 2. FİYAT VE SÜRE HESAPLAMA
            var policy = db.InsurancePolicies.Find(p.PolicyID);
            decimal basePrice = policy.Price;
            decimal finalPrice = 0;

            // Süre Çarpanları
            switch (Duration)
            {
                case 6: finalPrice = basePrice * 0.6m; break; // 6 Aylık (%60 fiyat)
                case 12: finalPrice = basePrice * 1.0m; break; // 1 Yıllık (Tam fiyat)
                case 24: finalPrice = basePrice * 1.8m; break; // 2 Yıllık (%10 indirimli x2)
                default: finalPrice = basePrice; break;
            }

            p.SaleDate = DateTime.Now;
            p.Amount = finalPrice;

            // 3. SATIŞI KAYDET
            db.Sales.Add(p);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
