using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Zavin.Slideshow.wpf
{
    class MainController
    {
        DatabaseController db = new DatabaseController();
        public void Echo()
        {
            Console.WriteLine("I am awake");
        }

        public List<ProductionDataModel> GetProduction()
        {
            var ProductionData = db.GetProductionTable();
            return ProductionData;
        }

        public List<KeyValuePair<string, int>> GetAcaf()
        {
            var AcafData = db.GetAcafTable();
            return AcafData;
        }

        public List<KeyValuePair<string, int>> GetPie()
        {
            var PieData = db.ParsePieData();
            return PieData;
        }
    }
}
