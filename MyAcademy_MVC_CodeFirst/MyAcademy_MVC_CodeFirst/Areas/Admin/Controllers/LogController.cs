using MyAcademy_MVC_CodeFirst.Filters;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class LogController : Controller
    {
        private readonly ElasticClient _elasticClient;

        public LogController()
        {
            // Elastic Bağlantısı
            // "lifesure-logs*" diyerek tüm tarihli log dosyalarını aramasını söylüyoruz.
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("lifesure-logs*");

            _elasticClient = new ElasticClient(settings);
        }

        [LogAction(ActionDescription = "Log Sayfası Görüntülendi")]
        public ActionResult Index()
        {
            // Elastic'ten Son 100 logu çek (Zamana göre tersten sırala)
            var searchResponse = _elasticClient.Search<dynamic>(s => s
                .Size(100)
                .Sort(ss => ss.Descending("@timestamp"))
                .Query(q => q.MatchAll())
            );

            // Gelen veriyi listeye çevirip View'a gönder
            var logs = searchResponse.Documents.ToList();
            return View(logs);
        }
    }
}
