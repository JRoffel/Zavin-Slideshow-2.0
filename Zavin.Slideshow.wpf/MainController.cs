﻿using System;
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

        public List<KeyValuePair<string, int>> GetLine()
        {
            var LineData = db.GetLineGraph();
            return LineData;
        }

        public List<KeyValuePair<string, int>> GetZeroLine()
        {
            List<KeyValuePair<string, int>> ZeroList = new List<KeyValuePair<string, int>>();

            ZeroList.Add(new KeyValuePair<string, int>("53", 0));
            ZeroList.Add(new KeyValuePair<string, int>("52", 0));

            return ZeroList;
        }
    }
}
