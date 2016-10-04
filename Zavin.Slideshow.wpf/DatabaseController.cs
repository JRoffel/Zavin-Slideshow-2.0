using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;

namespace Zavin.Slideshow.wpf
{
    class DatabaseController
    {
        public Random random = new Random();
        private List<ProductionDataModel> ParseProductionTable(DataClasses1DataContext Zavindb)
        {
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            string Date = Year + "-01-01T00:00:00Z";
            string Days = DateTime.Parse(Date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            int ToCount;
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

            List<ProductionDataModel> WeekProductionTon = new List<ProductionDataModel>();
            DateTime Startdate = DateTime.Parse(Year.ToString() + "-01-01T00:00:00Z");
            DateTime Enddate = DateTime.Parse(Year.ToString() + "-01-0" + (ToCount + 2).ToString() + "T00:00:00Z");

            var ProductionTableOddWeek = from production in Zavindb.wachtboeks where production.wb_date >= Startdate && production.wb_date <= Enddate select new { verstoo = production.wb_verstoo, wasta = production.wb_wasta };
            int total = 0;
            int wasta = 0;

            foreach (var ProductionListingOddWeek in ProductionTableOddWeek)
            {
                if (ProductionListingOddWeek.verstoo != null)
                {
                    total += (int)ProductionListingOddWeek.verstoo;
                }

                if (ProductionListingOddWeek.wasta != null)
                {
                    if (ProductionListingOddWeek.wasta == 1 && wasta != 2)
                    {
                        wasta = 1;
                    }
                    else if(ProductionListingOddWeek.wasta == 2)
                    {
                        wasta = 2;
                    }
                }
            }

            WeekProductionTon.Add(new ProductionDataModel { Week = "53", Burned = total / 1000, Wasta = wasta });

            int WeekCounter = 0;
            bool Continue = true;
            while (Startdate.Year == DateTime.Now.Year && Continue == true)
            {
                total = 0;
                wasta = 0;
                WeekCounter += 1;
                Startdate = (Enddate).AddHours(1);
                Enddate = (Enddate).AddDays(7);

                if (Enddate.Year != DateTime.Now.Year)
                {
                    Enddate = DateTime.Parse(Year.ToString() + "-12-31T00:00:00Z");
                    Continue = false;
                }

                var ProductionTableWeek = from production in Zavindb.wachtboeks where production.wb_date >= Startdate && production.wb_date <= Enddate select new { verstoo = production.wb_verstoo, wasta = production.wb_wasta };

                foreach (var ProductionListing in ProductionTableWeek)
                {
                    if (ProductionListing.verstoo != null)
                    {
                        total += (int)ProductionListing.verstoo;
                    }

                    if (ProductionListing.wasta != null)
                    {
                        if (ProductionListing.wasta == 1 && wasta != 2)
                        {
                            wasta = 1;
                        }
                        else if (ProductionListing.wasta == 2)
                        {
                            wasta = 2;
                        }
                    }
                }

                WeekProductionTon.Add(new ProductionDataModel { Week = WeekCounter.ToString(), Burned = total / 1000, Wasta = wasta });
            }

            return WeekProductionTon;
        }



        private List<KeyValuePair<string, int>> ParseAcafTable(int Year, DataClasses1DataContext Zavindb)
        {
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

        public List<ProductionDataModel> GetProductionTable()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();

            var WeekProductionTon = ParseProductionTable(Zavindb);

            return WeekProductionTon;
        }

        public List<KeyValuePair<string, int>> GetAcafTable()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

            var WeekProductionTon = ParseAcafTable(Year, Zavindb);

            return WeekProductionTon;
        }

        public int GetPieProduction()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            DateTime startdate = DateTime.Parse(Year.ToString() + "-01-01T00:00:00Z");
            DateTime enddate = DateTime.Parse(Year.ToString() + "-12-31T00:00:00Z");
            int total = 0;

            var Yearproduction = from production in Zavindb.wachtboeks where production.wb_date >= startdate && production.wb_date <= enddate select new { verstoo = production.wb_verstoo, date = production.wb_date };
            
            foreach (var Datapoint in Yearproduction)
            {
                if (Datapoint.verstoo != null)
                {
                    total += (int)Datapoint.verstoo;
                }
            }

            total /= 1000;

            return total;
        }

        private int GetPieTarget()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();

            var YearTarget = from production in Zavindb.configs select production.YearTargetTon;
            int target = 0;
            foreach (var Target in YearTarget)
            {
                target = Target.Value;
            }

            return target;        
        }

        public List<KeyValuePair<string, int>> ParsePieData()
        {
            int Production = GetPieProduction();
            int Target = GetPieTarget();
            int CalculatedProduction = 0;
            int CalculatedTarget = 0;
            int CalculatedExtra = 0;
            List<KeyValuePair<string, int>> DataList = new List<KeyValuePair<string, int>>();

            if (Production < Target)
            {
                CalculatedProduction = Production;
                CalculatedTarget = Target - Production;
            }
            else if (Production > Target)
            {
                CalculatedExtra = Production - Target;
                CalculatedProduction = Target - CalculatedExtra;
            }
            else
            {
                CalculatedProduction = Production;
            }

            DataList.Add(new KeyValuePair<string, int>("Begroting", CalculatedTarget));
            DataList.Add(new KeyValuePair<string, int>("Productie", CalculatedProduction));
            DataList.Add(new KeyValuePair<string, int>("Extra", CalculatedExtra));

            return DataList;
        }

        public double GetWeekTarget()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();

            var Yeartarget = from target in Zavindb.configs where target.Id == 1 select new { year = target.YearTargetTon };

            double WeekTarget = 0;
            var WeeksInYear = 0;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = new DateTime(Convert.ToInt32(DateTime.Now.Year), 12, 31);
            Calendar cal = dfi.Calendar;
            WeeksInYear = cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            foreach (var target in Yeartarget)
            {
                WeekTarget = (int)target.year / WeeksInYear;
            }

            return WeekTarget;
        }

        public List<KeyValuePair<string, int>> GetLineGraph()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();

            var LineGraphData = ParseLineData(Zavindb);

            return LineGraphData;
        }

        private List<KeyValuePair<string, int>> ParseLineData(DataClasses1DataContext Zavindb)
        {
            int Year = Convert.ToInt32(DateTime.Now.Year);
            string Date = Year + "-01-01T00:00:00Z";
            string Days = DateTime.Parse(Date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            double WeekTarget = GetWeekTarget();
            int ToCount;
            List<KeyValuePair<string, int>> LineListTon = new List<KeyValuePair<string, int>>();
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

            DateTime Startdate = DateTime.Parse(Year.ToString() + "-01-01T00:00:00Z");
            DateTime Enddate = DateTime.Parse(Year.ToString() + "-01-0" + (ToCount + 2).ToString() + "T00:00:00Z");

            var LineDataOddWeek = from production in Zavindb.wachtboeks where production.wb_date >= Startdate && production.wb_date <= Enddate select new { verstoo = production.wb_verstoo };
            int total = 0;
            double LastWeek = 0;
            double OddWeekTarget = (WeekTarget / 7) * (ToCount + 1);
            int CurrentWeekYear = 0;
            foreach(var LineListingOddWeek in LineDataOddWeek)
            {
                if(LineListingOddWeek.verstoo != null)
                {
                    total += (int)LineListingOddWeek.verstoo;
                }
            }
            LastWeek = (total / 1000) - OddWeekTarget;
            LineListTon.Add(new KeyValuePair<string, int>("53", (int)LastWeek));

            int WeekCounter = 0;
            bool Continue = true;

            while(Startdate.Year == DateTime.Now.Year && Continue == true)
            {
                WeekCounter++;
                total = 0;
                Startdate = (Enddate).AddHours(1);
                Enddate = (Enddate).AddDays(7);

                if (Enddate > DateTime.Now)
                {
                    Enddate = DateTime.Now;
                    CurrentWeekYear = GetCurrentWeek(Enddate);
                    Continue = false;
                }

                var LineDataWeek = from production in Zavindb.wachtboeks where production.wb_date >= Startdate && production.wb_date <= Enddate select new { verstoo = production.wb_verstoo };

                foreach (var LineListingWeek in LineDataWeek)
                {
                    if(LineListingWeek.verstoo != null)
                    {
                        total += (int)LineListingWeek.verstoo;
                    }
                }

                LastWeek = LastWeek + ((total / 1000) - WeekTarget);
                Console.WriteLine("LINE: {0}, {1}", LastWeek, WeekTarget);
                LineListTon.Add(new KeyValuePair<string, int>(WeekCounter.ToString(), Convert.ToInt32(LastWeek)));
            }

            if (CurrentWeekYear < 52)
            {
                int Weekstofill = 52 - CurrentWeekYear;
                for (int i = 0; i <= Weekstofill; i++)
                {
                    WeekCounter++;
                    LineListTon.Add(new KeyValuePair<string, int>(WeekCounter.ToString(), 0));
                }
            }

            return LineListTon;
        }

        private int GetCurrentWeek(DateTime date)
        {
            DateTime CurrentDate = date;

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(CurrentDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public int GetSlideTimerSeconds()
        {
            DataClasses1DataContext Zavindb = new DataClasses1DataContext();

            var SlideTimerResult = from config in Zavindb.configs select new { Timer = config.SlideTimerSeconds };
            int result = 30;

            foreach (var SlideTimer in SlideTimerResult)
            {
                if (SlideTimer.Timer != null && SlideTimer.Timer != 0)
                {
                    result = (int)SlideTimer.Timer;
                }
            }

            return result;
        }
    }
}