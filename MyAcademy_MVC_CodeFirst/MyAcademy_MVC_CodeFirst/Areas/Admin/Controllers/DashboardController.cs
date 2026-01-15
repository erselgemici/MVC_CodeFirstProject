using MyAcademy_MVC_CodeFirst.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        AppDbContext db = new AppDbContext();

        MlService mlService = new MlService();

        public ActionResult Index()
        {
            // --- 1. KART BİLGİLERİ ---
            ViewBag.TotalSales = db.Sales.Any() ? db.Sales.Sum(x => x.Amount) : 0;
            ViewBag.TotalCustomers = db.Customers.Count();
            ViewBag.TotalPolicies = db.InsurancePolicies.Count();
            ViewBag.TotalMessages = db.ContactMessages.Count();

            // --- 2. ANA GRAFİK (CİRO TRENDİ) ---
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var pastDataRaw = db.Sales
                .Where(x => x.SaleDate < currentMonth)
                .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
                .Select(g => new { Date = g.Key.Year + "-" + g.Key.Month, Amount = g.Sum(x => x.Amount) })
                .ToList()
                .Select(x => new MyAcademy_MVC_CodeFirst.Models.SalesData { Date = Convert.ToDateTime(x.Date + "-01"), Value = (float)x.Amount })
                .OrderBy(x => x.Date).ToList();

            var futureDataRaw = mlService.PredictTotalRevenue();

            ViewBag.PastData = pastDataRaw;
            ViewBag.FutureData = futureDataRaw;

            // --- 3. ŞEHİR KIYASLAMASI (GEÇMİŞ vs GELECEK) ---
            var topCities = db.Sales
                .GroupBy(x => x.Customer.City)
                .Select(g => new { City = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(4) // Ekrana sığsın diye 4 şehir
                .ToList();

            var cityLabels = new List<string>();
            var cityPastValues = new List<float>(); // Geçmiş 3 Ayın Ortalaması/Toplamı
            var cityFutureValues = new List<float>(); // Yapay Zeka Tahmini

            // Geçmiş 3 ayın başlangıç tarihi
            var threeMonthsAgo = DateTime.Now.AddMonths(-3);

            foreach (var item in topCities)
            {
                if (!string.IsNullOrEmpty(item.City))
                {
                    cityLabels.Add(item.City);

                    // A) Geçmiş Veri (Son 90 gün)
                    var pastCount = db.Sales.Count(x => x.Customer.City == item.City && x.SaleDate >= threeMonthsAgo);
                    cityPastValues.Add(pastCount);

                    // B) Gelecek Tahmini (AI)
                    float prediction = mlService.PredictCityNext3Months(item.City);
                    cityFutureValues.Add((float)Math.Round(prediction));
                }
            }

            ViewBag.CityLabels = cityLabels;
            ViewBag.CityPastValues = cityPastValues;
            ViewBag.CityFutureValues = cityFutureValues;

            // --- 4. ÜRÜN PASTASI (İsimler Görünecek) ---
            var topProducts = db.Sales
                .GroupBy(x => x.InsurancePolicy.PolicyName)
                .Select(g => new { Product = g.Key, Amount = g.Sum(x => x.Amount) })
                .OrderByDescending(x => x.Amount)
                .Take(5)
                .ToList();
            ViewBag.ProductLabels = topProducts.Select(x => x.Product).ToList();
            ViewBag.ProductValues = topProducts.Select(x => x.Amount).ToList();

            // --- 5. YENİ: KATEGORİ RADAR GRAFİĞİ (Risk Analizi Gibi Durur) ---
            // Hangi kategoride (Sağlık, Araç, Konut) ne kadar güçlüyüz?
            // Not: Policy tablosunda CategoryID olduğunu varsayıyorum, yoksa PolicyName'den grupla
            var categoryStats = db.Sales
                .GroupBy(x => x.InsurancePolicy.PolicyName) // Kategori tablosu joinli değilse PolicyName kullan
                .Select(g => new { Cat = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            ViewBag.RadarLabels = categoryStats.Select(x => x.Cat).ToList();
            ViewBag.RadarValues = categoryStats.Select(x => x.Count).ToList();


            // --- 6. SON İŞLEMLER ---
            var lastSales = db.Sales.Include("Customer").Include("InsurancePolicy")
                            .OrderByDescending(x => x.SaleDate)
                            .Take(6)
                            .ToList();

            return View(lastSales);
        }
    }
}
