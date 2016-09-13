using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;

namespace Zavin.Slideshow.wpf
{
    class DatabaseController
    {
        public List<KeyValuePair<string, int>> ParseProductionTable(dynamic TableToParse)
        {
            int[] Months = new int[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (DateTime.IsLeapYear(Convert.ToInt32(DateTime.Now.ToString("yyyy"))) == true)
            {
                Months[2] = 29;
            }
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            string Date = Year + "-01-01T00:00:00Z";
            string Days = DateTime.Parse(Date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
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

            foreach (var productionData in TableToParse)
            {
                if (Counter != 0)
                {
                    if (productionData.verstoo == null)
                    {
                        Total += 0;
                        WeekProduction[IndexCounter] = new KeyValuePair<string, int>(WeekCounter.ToString(), Total);
                        Counter -= 1;
                    }
                    else
                    {
                        Total += (int)productionData.verstoo;
                        WeekProduction[IndexCounter] = new KeyValuePair<string, int>(WeekCounter.ToString(), Total);
                        Counter -= 1;
                    }
                }
                else if (IsInitialRun)
                {
                    Counter = ToCount - 1;
                    IsInitialRun = false;
                    WeekCounter += 1;
                    IndexCounter += 1;
                    if (productionData.verstoo == null)
                    {
                        Total += 0;
                        WeekProduction[IndexCounter] = new KeyValuePair<string, int>(WeekCounter.ToString(), Total);
                    }
                    else
                    {
                        Total += (int)productionData.verstoo;
                        WeekProduction.Add(new KeyValuePair<string, int>(WeekCounter.ToString(), Total));
                    }             
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
                    if (productionData.verstoo == null)
                    {
                        Total += 0;
                        WeekProduction[IndexCounter] = new KeyValuePair<string, int>(WeekCounter.ToString(), Total);
                    }
                    else
                    {
                        Total += (int)productionData.verstoo;
                        WeekProduction.Add(new KeyValuePair<string, int>(WeekCounter.ToString(), Total));
                    }
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
        }

        public List<KeyValuePair<string, int>> ParseAcafTable(int Year, DataClasses1DataContext Zavindb)
        {
            int[] Months = new int[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (DateTime.IsLeapYear(Convert.ToInt32(DateTime.Now.ToString("yyyy"))) == true)
            {
                Months[2] = 29;
            }
            else
            {
                Months[2] = 28;
            }
            string Date = Year + "-01-01T00:00:00Z";
            string Days = DateTime.Parse(Date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            int ToCount;
            List<KeyValuePair<string, int>> AcafTonList = new List<KeyValuePair<string, int>>();
            switch (Days)
            {
                case "ma":
                    ToCount = 6;
                    break;
                case "di":
                    ToCount = 5;
                    break;
                case "wo":
                    ToCount = 4;
                    break;
                case "do":
                    ToCount = 3;
                    break;
                case "vr":
                    ToCount = 2;
                    break;
                case "za":
                    ToCount = 1;
                    break;
                case "zo":
                    ToCount = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            List<KeyValuePair<string, int>> WeekProduction = new List<KeyValuePair<string, int>>();
            DateTime Startdate = DateTime.Parse(Year.ToString() + "-01-01T00:00:00Z");
            DateTime Enddate = DateTime.Parse(Year.ToString() + "-01-0" + (ToCount + 2).ToString() + "T00:00:00Z");

            var AcafTableOddWeek = from acaf in Zavindb.acafs where acaf.acaf_datum >= Startdate && acaf.acaf_datum <= Enddate select new { date = acaf.acaf_datum, gewge = acaf.acaf_gewge };
            int total = 0;
            foreach (var AcafListingOddWeek in AcafTableOddWeek)
            {
                if(AcafListingOddWeek.gewge != null)
                {
                    total += (int)AcafListingOddWeek.gewge;
                }
            }
            AcafTonList.Add(new KeyValuePair<string, int> ( "53", (total/1000)));

            int WeekCounter = 0;
            bool Continue = true;
            while (Startdate.Year == DateTime.Now.Year && Continue == true)
            {
                total = 0;
                WeekCounter += 1;
                Startdate = (Enddate).AddHours(1);
                Enddate = (Enddate).AddDays(7);

                if (Enddate.Year != DateTime.Now.Year)
                {
                    Enddate = DateTime.Parse(Year.ToString() + "-12-31T00:00:00Z");
                    Continue = false;
                }

                var AcafTable = from acaf in Zavindb.acafs where acaf.acaf_datum >= Startdate && acaf.acaf_datum <= Enddate select new { date = acaf.acaf_datum, gewge = acaf.acaf_gewge };
                
                foreach(var AcafListing in AcafTable)
                {
                    if(AcafListing.gewge != null)
                    {
                        total += (int)AcafListing.gewge;
                    }
                }

                AcafTonList.Add(new KeyValuePair<string, int>(WeekCounter.ToString(), (total/1000)));
            }

            return AcafTonList;
        }

        public List<KeyValuePair<string, int>> GetProductionTable()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            DateTime Startdate = DateTime.Parse((Year - 1).ToString() + "-12-31T23:59:59Z");
            DateTime Enddate = DateTime.Parse((Year).ToString() + "-12-31T23:59:59Z");

            var ProductionTable = from production in Zavindb.wachtboeks where production.wb_date >= Startdate && production.wb_date <= Enddate orderby production.wb_date select new { date = production.wb_date, verstoo = production.wb_verstoo, wasta = production.wb_wasta };
            var WeekProductionTon = ParseProductionTable(ProductionTable);
            foreach(var Prod in WeekProductionTon)
            {
                Console.WriteLine("KEY: {0}, VALUE:{1}", Prod.Key, Prod.Value);
            }
            return WeekProductionTon;
        }

        public List<KeyValuePair<string, int>> GetAcafTable()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

            var WeekProductionTon = ParseAcafTable(Year, Zavindb);

            return WeekProductionTon;
        }
    }
}