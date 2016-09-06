using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Zavin.Slideshow.wpf
{
    class DatabaseController
    {
        public List<KeyValuePair<string, int>> GetProductionTable()
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

            var ProductionTable = from production in Zavindb.wachtboeks where production.wb_date >= Startdate && production.wb_date <= Enddate select new { date = production.wb_date, verstoo = production.wb_verstoo, wasta = production.wb_wasta };

            int[] Months = new int[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (DateTime.IsLeapYear(Convert.ToInt32(DateTime.Now.ToString("yyyy"))) == true)
            {
                Months[2] = 29;
            }
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            string Date = Year + "-01-01T00:00:00Z";
            string Days = DateTime.Parse(Date).ToString("ddd");
            int ToCount;
            switch (Days)
            {
                case "ma":
                    ToCount = 21;
                    break;
                case "di":
                    ToCount = 18;
                    break;
                case "wo":
                    ToCount = 15;
                    break;
                case "do":
                    ToCount = 12;
                    break;
                case "vr":
                    ToCount = 9;
                    break;
                case "za":
                    ToCount = 6;
                    break;
                case "zo":
                    ToCount = 3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            List<KeyValuePair<string, int>> WeekProduction = new List<KeyValuePair<string, int>>();
            int Counter = 0;
            int Total = 0;
            int WeekCounter = 0;
            int IndexCounter = -1;
            bool IsInitialRun = true;
            bool IsInitialRound = true;

            foreach (var productionData in ProductionTable)
            {
                if (Counter != 0)
                {
                    Total += (int)productionData.verstoo;
                    WeekProduction[IndexCounter] = new KeyValuePair<string, int>(WeekCounter.ToString(), Total);
                    Counter -= 1;
                }
                else if (IsInitialRun)
                {
                    Counter = ToCount -1;
                    IsInitialRun = false;
                    WeekCounter += 1;
                    IndexCounter += 1;
                    Total += (int)productionData.verstoo;
                    WeekProduction.Add(new KeyValuePair<string, int>(WeekCounter.ToString(), Total));
                }
                else
                {
                    if (IsInitialRound && Days != "ma")
                    {
                        WeekCounter = 0;
                        IndexCounter = 0;
                        Total = WeekProduction[0].Value;
                        WeekProduction[0] = new KeyValuePair<string, int>("53", Total);
                        IsInitialRound = false;
                    }
                    Total = 0;
                    Counter = 20;
                    WeekCounter += 1;
                    IndexCounter += 1;
                    Total += (int)productionData.verstoo;
                    WeekProduction.Add(new KeyValuePair<string, int>(WeekCounter.ToString(), Total));
                }
            }

            List<KeyValuePair<string, int>> WeekProductionTon = new List<KeyValuePair<string, int>>();

            foreach (KeyValuePair<string, int> pair in WeekProduction)
            {
                //Console.WriteLine("KEY:{0}, VALUE:{1}", pair.Key, pair.Value);
                int Temp = pair.Value;
                Temp = (int)Math.Ceiling((decimal)Temp / 1000);
                WeekProductionTon.Add(new KeyValuePair<string, int>(pair.Key, Temp));                
            }

            return WeekProductionTon;

            //Console.WriteLine(Enumerable.Count(table));

            //Console.WriteLine(table);
        }
    }
}