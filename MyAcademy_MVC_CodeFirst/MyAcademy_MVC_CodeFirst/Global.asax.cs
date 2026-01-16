using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyAcademy_MVC_CodeFirst
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName() // Hangi bilgisayar?
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
        {
            AutoRegisterTemplate = true, // Elastic'te tabloyu (index) otomatik oluştur
            // Loglar gün gün ayrılsın: lifesure-logs-2026.01.17 gibi
            IndexFormat = "lifesure-logs-{0:yyyy.MM.dd}",
            NumberOfShards = 2,
            NumberOfReplicas = 1
        })
        .CreateLogger();


            System.Data.Entity.Database.SetInitializer<MyAcademy_MVC_CodeFirst.Data.Context.AppDbContext>(null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
    }
}
