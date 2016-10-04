using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Zavin.Slideshow.wpf
{
    public class MainController
    {
        DatabaseController db = new DatabaseController();
        public void Echo()
        {
            Console.WriteLine("I am awake");
        }

        public List<ProductionDataModel> GetProduction()
        {
            var ProductionData = db.GetProductionTable();
            ProductionData[27].Wasta = 1;
            ProductionData[27].Burned = 290;
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

            if(db.random.Next() % 2 == 0)
            {
                PieData[0] = new KeyValuePair<string, int>("Begroting", 0);
                PieData[1] = new KeyValuePair<string, int>("Productie", 9750);
                PieData[2] = new KeyValuePair<string, int>("Extra", 250);
            }

            return PieData;
        }

        public List<KeyValuePair<string, int>> GetLine()
        {
            var LineData = db.GetLineGraph();
            return LineData;
        }

        public List<KeyValuePair<string, int>> GetZeroLine()
        {
            List<KeyValuePair<string, int>> ZeroList = new List<KeyValuePair<string, int>>();

            ZeroList.Add(new KeyValuePair<string, int>("53", 0));
            for(int i = 1; i < 53; i++)
            {
                ZeroList.Add(new KeyValuePair<string, int>(i.ToString(), 0));
            }

            return ZeroList;
        }

        public int GetProdPie()
        {
            int total = db.GetPieProduction();
            return total;
        }

        public int GetSlideTimer()
        {
            int slideTimerSeconds = db.GetSlideTimerSeconds();
            return (slideTimerSeconds * 1000);
        }
    }
}
