using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Zavin.Slideshow.wpf
{
    class DatabaseController
    {
        public void GetProductionTable()
        {
            //DataContext Zavindb = new DataContext("192.168.187.160/mczavidord");
            //Table<ProductionDatabaseModel> Production = Zavindb.GetTable<ProductionDatabaseModel>();

            //Zavindb.Log = Console.Out;

            //IQueryable<ProductionDatabaseModel> ProductionQuery =
            //    from production in Production
            //    select production;

            //foreach (ProductionDatabaseModel production in ProductionQuery)
            //{
            //    Console.WriteLine("ID={0}, DATE={1}, VERSTOO={2}, WASTA={3}", production.wb_nmr, production.wb_date, production.wb_verstoo, production.wb_wasta);
            //}

            DataClasses1DataContext Zavindb = new DataClasses1DataContext();

            DateTime Startdate = DateTime.Parse("2015-12-31T23:59:59Z");
            DateTime Enddate = DateTime.Parse("2017-01-01T00:00:01");

            var ProductionTable = from production in Zavindb.wachtboeks where production.wb_date >= Startdate && production.wb_date <= Enddate select production;

            foreach (var ProductionData in ProductionTable)
            {
                Console.WriteLine("ID={0}, DATE={1}, VERSTOO={2}, WASTA={3}", ProductionData.wb_nmr, ProductionData.wb_date, ProductionData.wb_verstoo, ProductionData.wb_wasta);
            }

            //Console.WriteLine(Enumerable.Count(table));

            //Console.WriteLine(table);
        }
    }
}