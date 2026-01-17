using MyAcademy_MVC_CodeFirst.Filters;
using Nest; // ELASTICSEARCH: C# ile Elastic'in konuşmasını sağlayan kütüphane.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class LogController : Controller
    {
        private readonly ElasticClient _elasticClient;

        public LogController()
        {
            // DefaultIndex: Eğer indeks adı vermezsem otomatik "lifesure-logs*" ile başlayanlara bak.
            // Sondaki (*) işareti wildcard'dır. Yani "lifesure-logs-2025", "lifesure-logs-2026" hepsini kapsar.
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("lifesure-logs*");

            _elasticClient = new ElasticClient(settings);
        }

        [LogAction(ActionDescription = "Log Sayfası Görüntülendi")]
        public ActionResult Index(string search = "", string startDate = "", string endDate = "")
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
            var client = new ElasticClient(settings);

            // Gelen string tarihleri (HTML input'tan gelir) DateTime formatına çeviriyoruz.
            DateTime? start = null;
            DateTime? end = null;
            if (!string.IsNullOrEmpty(startDate)) start = DateTime.Parse(startDate);
            if (!string.IsNullOrEmpty(endDate)) end = DateTime.Parse(endDate);

            // SQL'deki "SELECT * FROM Logs WHERE ..." sorgusunun Elasticçesi.
            var searchResponse = client.Search<dynamic>(s => s
                .Index("lifesure-logs-*") // Hangi tablolara (indekslere) bakayım? Hepsine (*).
                .Size(1000) // PERFORMANS: Son 1000 tane log.
                .Sort(ss => ss.Descending("@timestamp"))
                .Query(q => q
                    .Bool(b => b // (Hem arama, hem tarih filtresi)
                        .Must(m =>
                            // METİN ARAMA (Search Box)
                            // Eğer arama kutusu doluysa içinde geçen kelimeyi ara (*text*), boşsa hepsini getir.
                            !string.IsNullOrEmpty(search)
                                ? m.QueryString(qs => qs.Query("*" + search + "*")) // SQL'deki LIKE '%search%'
                                : m.MatchAll()
                        )
                        .Filter(f => f
                            // TARİH ARALIĞI (Date Range)
                            // SQL'deki BETWEEN start AND end
                            .DateRange(r => r
                                .Field("@timestamp") // Logun atıldığı zaman
                                .GreaterThanOrEquals(start) // Başlangıç tarihinden büyük olsun
                                .LessThanOrEquals(end?.AddDays(1)) // Bitiş gününü kapsasın diye +1 gün ekliyoruz.
                            )
                        )
                    )
                )
            );

            // DASHBOARD KARTLARI İÇİN İSTATİSTİK

            // Bugün kaç log atılmış?
            var totalLogsToday = client.Count<dynamic>(c => c.Index("lifesure-logs-*").Query(q => q.DateRange(r => r.Field("@timestamp").GreaterThanOrEquals(DateTime.Today))));

            // Toplam kaç tane hata (Error) var?
            var totalErrors = client.Count<dynamic>(c => c.Index("lifesure-logs-*").Query(q => q.Match(m => m.Field("level").Query("Error"))));

            ViewBag.TotalLogsToday = totalLogsToday.Count;
            ViewBag.TotalErrors = totalErrors.Count;

            // Kullanıcı "Filtrele"ye bastığında sayfa yenilenir.
            ViewBag.SearchTerm = search;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            // Elastic'ten gelen JSON dökümanlarını listeye çevirip View'a atıyoruz.
            var logList = searchResponse.Documents.ToList();
            return View(logList);
        }

        // RAPORLAMA: Logları Excel (CSV) olarak indirme
        public ActionResult ExportExcel()
        {
            var client = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200")));

            // Ekranda 1000 tane gösteriyoruz ama raporda son 5000 taneyi veriyoruz.
            var response = client.Search<dynamic>(s => s.Index("lifesure-logs-*").Size(5000).Sort(ss => ss.Descending("@timestamp")));
            var logs = response.Documents.ToList();

            // STRINGBUILDER:
            // Binlerce satırı "string + string" yaparak birleştirmek RAM'i şişirir.
            // StringBuilder bu iş için optimize edilmiştir.
            var sb = new StringBuilder();

            // CSV Başlık Satırı (Excel sütunları)
            sb.AppendLine("Tarih,Seviye,Mesaj,Kullanıcı,İşlem,Detay");

            foreach (var log in logs)
            {
                // Elastic verisi "dynamic" olduğu için onu Sözlük (Dictionary) gibi ele alıyoruz.
                // Böylece ["key"] yazarak verilere ulaşabiliriz.
                var dict = log as IDictionary<string, object>;

                // Veri yoksa patlamasın, boş string ("") dönsün.
                string time = dict.ContainsKey("@timestamp") ? dict["@timestamp"].ToString() : "";
                string level = dict.ContainsKey("level") ? dict["level"].ToString() : "";

                // CSV FORMATI: Sütunları virgül (,) ile ayırarak yan yana yazıyoruz.
                sb.AppendLine($"{time},{level},Log Detayı,...,...");
            }

            // DOSYAYI İNDİR
            // Türkçe karakter sorunu olmasın diye UTF8 Encoding kullanıyoruz.
            // MIME Type: "text/csv" (Tarayıcı bunun bir Excel/CSV dosyası olduğunu anlar).
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "SistemLoglari.csv");
        }
    }
}
