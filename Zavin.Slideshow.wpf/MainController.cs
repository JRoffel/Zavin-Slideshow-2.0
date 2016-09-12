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
            Console.WriteLine("Echo Echo... I am alive");
        }

        public List<KeyValuePair<string, int>> GetProduction()
        {
            var ProductionData = db.GetProductionTable();
            return ProductionData;
        }

        public List<KeyValuePair<string, int>> GetAcaf()
        {
            var AcafData = db.GetAcafTable();
            return AcafData;
        }
    }
}
