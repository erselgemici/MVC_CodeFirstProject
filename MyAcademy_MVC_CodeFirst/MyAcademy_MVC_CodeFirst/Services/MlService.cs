using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Models; 
using System;
using System.Collections.Generic;
using System.Linq;

public class MlService
{
    private readonly AppDbContext _context;

    public MlService()
    {
        _context = new AppDbContext();
    }

    // Şehir bazlı gelecek 3 ayı tahmin et
    // Geri dönüş tipi değişti: Hem tarih hem değer listesi dönecek
    public List<SalesData> PredictTotalRevenue()
    {
        var mlContext = new MLContext();

        // 1. TÜM SATIŞLARI ÇEK (Şehir ayrımı yok)
        var rawData = _context.Sales
            .Select(x => new { x.SaleDate, x.Amount })
            .ToList();

        if (!rawData.Any()) return new List<SalesData>();

        // 2. AYLIK TOPLAM CİRO (Amount Sum)
        var monthlyData = rawData
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            .Select(g => new SalesData
            {
                Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                Value = (float)g.Sum(x => x.Amount) // CİRO TAHMİNİ (Daha havalı rakamlar)
            })
            .OrderBy(x => x.Date)
            .ToList();

        // Veri azsa işlem yapma
        if (monthlyData.Count < 12) return new List<SalesData>();

        // 3. MODELİ EĞİT (SSA)
        IDataView dataView = mlContext.Data.LoadFromEnumerable(monthlyData);

        var pipeline = mlContext.Forecasting.ForecastBySsa(
            outputColumnName: nameof(SalesPrediction.Forecast),
            inputColumnName: nameof(SalesData.Value),
            windowSize: 5,       // Pencereyi büyüttük
            seriesLength: 12,    // 12 Aylık döngü
            trainSize: monthlyData.Count,
            horizon: 6,          // GELECEK 6 AYI TAHMİN ET
            confidenceLevel: 0.95f,
            confidenceLowerBoundColumn: nameof(SalesPrediction.LowerBound),
            confidenceUpperBoundColumn: nameof(SalesPrediction.UpperBound)
        );

        var model = pipeline.Fit(dataView);
        var engine = model.CreateTimeSeriesEngine<SalesData, SalesPrediction>(mlContext);
        var forecast = engine.Predict();

        // 4. SONUÇLARI HAZIRLA
        var predictions = new List<SalesData>();
        var lastDate = monthlyData.Last().Date;

        for (int i = 0; i < 6; i++)
        {
            predictions.Add(new SalesData
            {
                Date = lastDate.AddMonths(i + 1),
                Value = Math.Max(0, forecast.Forecast[i])
            });
        }

        return predictions;
    }

    // Bir şehrin gelecek 3 aydaki TOPLAM satış adedini tahmin eder
    public float PredictCityNext3Months(string city)
    {
        var mlContext = new MLContext();

        // 1. O şehre ait verileri çek
        var rawData = _context.Sales
            .Where(x => x.Customer.City == city)
            .Select(x => new { x.SaleDate, x.Amount }) // Adet sayacağız
            .ToList();

        // Veri yoksa veya çok azsa 0 dön
        if (rawData.Count < 10) return 0;

        // 2. Aylık Gruplama (Adet Bazlı)
        var monthlyData = rawData
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            .Select(g => new SalesData
            {
                Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                Value = (float)g.Count() // Adet sayıyoruz
            })
            .OrderBy(x => x.Date)
            .ToList();

        // 3. Model Eğitimi (Hızlı ve Basit SSA)
        // Veri seti küçük olabileceği için windowSize düşürüldü
        if (monthlyData.Count < 5) return 0;

        IDataView dataView = mlContext.Data.LoadFromEnumerable(monthlyData);

        var pipeline = mlContext.Forecasting.ForecastBySsa(
            outputColumnName: nameof(SalesPrediction.Forecast),
            inputColumnName: nameof(SalesData.Value),
            windowSize: 2,
            seriesLength: monthlyData.Count,
            trainSize: monthlyData.Count,
            horizon: 3, // Gelecek 3 Ay
            confidenceLevel: 0.95f,
            confidenceLowerBoundColumn: nameof(SalesPrediction.LowerBound),
            confidenceUpperBoundColumn: nameof(SalesPrediction.UpperBound)
        );

        var model = pipeline.Fit(dataView);
        var engine = model.CreateTimeSeriesEngine<SalesData, SalesPrediction>(mlContext);
        var forecast = engine.Predict();

        // 4. Gelecek 3 ayın toplamını döndür
        float totalPrediction = 0;
        foreach (var val in forecast.Forecast)
        {
            totalPrediction += Math.Max(0, val); // Negatif çıkarsa 0 al
        }

        return totalPrediction;
    }
}
