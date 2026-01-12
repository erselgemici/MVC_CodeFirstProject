using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class SalesController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index(string search, int page = 1)
        {
            var values = db.Sales.Include("Customer").Include("InsurancePolicy").AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                values = values.Where(x => x.Customer.FirstName.Contains(search) ||
                                           x.Customer.LastName.Contains(search) ||
                                           x.InsurancePolicy.PolicyName.Contains(search));
            }
            ViewBag.Search = search;

            // Satışları en yeniden eskiye sıralayalım
            return View(values.OrderBy(x => x.SaleID).ToPagedList(page, 10));
        }

        [HttpGet]
        public ActionResult CreateSale()
        {
            List<SelectListItem> customers = (from x in db.Customers.OrderBy(c => c.FirstName).ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.FirstName + " " + x.LastName + " - " + x.City,
                                                  Value = x.CustomerID.ToString()
                                              }).ToList();

            List<SelectListItem> policies = (from x in db.InsurancePolicies.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.PolicyName + " (" + x.Price.ToString("C0") + ")",
                                                 Value = x.PolicyID.ToString()
                                             }).ToList();

            ViewBag.Customers = customers;
            ViewBag.Policies = policies;
            return View();
        }

        [HttpPost]
        public ActionResult CreateSale(Sale sale)
        {
            var policy = db.InsurancePolicies.Find(sale.PolicyID);
            if (policy != null)
            {
                sale.Amount = policy.Price; 
                sale.SaleDate = DateTime.Now;
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sale);
        }

        public ActionResult DeleteSale(int id)
        {
            var value = db.Sales.Find(id);
            if (value != null)
            {
                db.Sales.Remove(value);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateSale(int id)
        {
            var sale = db.Sales.Find(id);

            List<SelectListItem> customers = (from x in db.Customers.OrderBy(c => c.FirstName).ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.FirstName + " " + x.LastName,
                                                  Value = x.CustomerID.ToString()
                                              }).ToList();

            List<SelectListItem> policies = (from x in db.InsurancePolicies.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.PolicyName,
                                                 Value = x.PolicyID.ToString()
                                             }).ToList();

            ViewBag.Customers = customers;
            ViewBag.Policies = policies;

            return View(sale);
        }

        [HttpPost]
        public ActionResult UpdateSale(Sale sale)
        {
            var value = db.Sales.Find(sale.SaleID);
            value.CustomerID = sale.CustomerID;
            value.PolicyID = sale.PolicyID;

            // Poliçe değiştiyse fiyatı güncelle
            var policy = db.InsurancePolicies.Find(sale.PolicyID);
            value.Amount = policy.Price;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
