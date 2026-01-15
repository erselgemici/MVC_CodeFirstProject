using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAcademy_MVC_CodeFirst.Models
{
    public class SalesPrediction
    {
        // Tahmin edilen değerler dizisi (Örn: Gelecek 3 ayın tahminleri)
        [VectorType(3)]
        public float[] Forecast { get; set; }

        // Alt sınır (Güven aralığı - En kötü senaryo)
        [VectorType(3)]
        public float[] LowerBound { get; set; }

        // Üst sınır (Güven aralığı - En iyi senaryo)
        [VectorType(3)]
        public float[] UpperBound { get; set; }
    }
}
