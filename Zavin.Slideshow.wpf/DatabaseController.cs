using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
// ReSharper disable AccessToModifiedClosure

namespace Zavin.Slideshow.wpf
{
    internal class DatabaseController
    {
        private static List<ProductionDataModel> ParseProductionTable(DataClasses1DataContext zavindb, int year)
        {
            var isPrevious = year == DateTime.Now.Year - 1;
            var date = year + "-01-01T00:00:00Z";
            var days = DateTime.Parse(date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            int toCount;
            switch (days)
            {
                case "ma":
                    toCount = 6;
                    break;
                case "di":
                    toCount = 5;
                    break;
                case "wo":
                    toCount = 4;
                    break;
                case "do":
                    toCount = 3;
                    break;
                case "vr":
                    toCount = 2;
                    break;
                case "za":
                    toCount = 1;
                    break;
                case "zo":
                    toCount = 0;
                    break;
                default:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            var weekProductionTon = new List<ProductionDataModel>();
            var startdate = DateTime.Parse(year + "-01-01T00:00:00Z");
            var enddate = DateTime.Parse(year + "-01-0" + (toCount + 1) + "T00:00:00Z");

            var productionTableOddWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate && production.wb_date <= enddate select new { verstoo = production.wb_verstoo, wasta = production.wb_wasta };
            var total = 0;
            var wasta = 0;

            foreach (var productionListingOddWeek in productionTableOddWeek)
            {
                if (productionListingOddWeek.verstoo != null)
                {
                    total += (int)productionListingOddWeek.verstoo;
                }

                if (productionListingOddWeek.wasta != null)
                {
                    if (productionListingOddWeek.wasta == 1 && wasta != 2)
                    {
                        wasta = 1;
                    }
                    else if(productionListingOddWeek.wasta == 2)
                    {
                        wasta = 2;
                    }
                }
            }

            weekProductionTon.Add(new ProductionDataModel { Week = "53", Burned = total / 1000, Wasta = wasta });

            var weekCounter = 0;
            var Continue = true;
            while (((startdate.Year == DateTime.Now.Year && isPrevious == false) || (startdate.Year == DateTime.Now.Year - 1 && isPrevious)) && Continue)
            {
                total = 0;
                wasta = 0;
                weekCounter += 1;
                startdate = (enddate).AddHours(1);
                enddate = (enddate).AddDays(7);

                if ((enddate.Year != DateTime.Now.Year && isPrevious == false) || (enddate.Year != DateTime.Now.Year -1 && isPrevious))
                {
                    enddate = DateTime.Parse(year + "-12-31T00:00:00Z");
                    Continue = false;
                }

                var productionTableWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate && production.wb_date <= enddate select new { verstoo = production.wb_verstoo, wasta = production.wb_wasta };

                foreach (var productionListing in productionTableWeek)
                {
                    if (productionListing.verstoo != null)
                    {
                        total += (int)productionListing.verstoo;
                    }

                    if (productionListing.wasta != null)
                    {
                        if (productionListing.wasta == 1 && wasta != 2)
                        {
                            wasta = 1;
                        }
                        else if (productionListing.wasta == 2)
                        {
                            wasta = 2;
                        }
                    }
                }

                weekProductionTon.Add(new ProductionDataModel { Week = weekCounter.ToString(), Burned = total / 1000, Wasta = wasta });
            }

            return weekProductionTon;
        }

        public Memo GetWelcomePage()
        {
            var db = new DataClasses1DataContext();
            var welcome = ParseMemo(0, db, "welcome");
            return welcome;
        }

        public Memo GetMemo(int number)
        {
            var db = new DataClasses1DataContext();
            var memo = ParseMemo(number, db, "memo");
            return memo;
        }

        public int GetMemoCount()
        {
            var db = new DataClasses1DataContext();
            var memoCount = CountMemos(db);
            return memoCount;
        }

        private static Memo ParseMemo(int number, DataClasses1DataContext db, string type)
        {
            var date = DateTime.Now;
            // ReSharper disable once InconsistentNaming
            var Memo = new Memo();
            var iterator = 1;

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if(type == "memo")
            {
                var memoTableValidMemo = from memo in db.infopers where memo.info_date <= date && memo.info_date2 >= date && memo.info_type == true select new { title = memo.info_desc, desc = memo.info_comm, creation = memo.info_date, author = memo.info_craft, image = memo.info_bitmap };

                foreach (var memoItem in memoTableValidMemo)
                {
                    if (iterator == number)
                    {
                        Memo.Title = memoItem.title;
                        Memo.Description = memoItem.desc;
                        Memo.Author = memoItem.author;
                        Memo.PostDate = (DateTime)memoItem.creation;
                        Memo.ImagePath = memoItem.image;
                    }

                    iterator++;
                }
            }
            else if (type == "welcome")
            {
                var memoTableValidWelcome = from memo in db.infopers where memo.info_date <= date && memo.info_date2 >= date & memo.info_type2 == true select new { title = memo.info_desc, desc = memo.info_comm, image = memo.info_bitmap };

                foreach (var memoItem in memoTableValidWelcome)
                {
                    Memo.Title = memoItem.title;
                    Memo.Description = memoItem.desc;
                    Memo.ImagePath = memoItem.image;
                    Memo.Author = "Do not show";
                    Memo.PostDate = DateTime.MinValue;
                }
            }
            else
            {
                throw new NotImplementedException("No other Memo type than 'memo' or 'welcome' has been implemented");
            }

            return Memo;
        }

        private static int CountMemos(DataClasses1DataContext db)
        {
            var date = DateTime.Now;
            var memoCount = 0;

            var memotable = from memo in db.infopers where memo.info_date <= date && memo.info_date2 >= date && memo.info_type == true select memo;

            // ReSharper disable once UnusedVariable
            foreach(var memo in memotable)
            {
                memoCount++;
            }

            return memoCount;
        }

        public bool HasWelcomeScreen()
        {
            var db = new DataClasses1DataContext();
            var date = DateTime.Now;

            var welcometable = from memo in db.infopers where memo.info_date <= date && memo.info_date2 >= date && memo.info_type2 == true select memo;
            
            // ReSharper disable once UnusedVariable
            foreach (var welcome in welcometable)
            {
                return true;
            }

            return false;
        }

        private static List<KeyValuePair<string, int>> ParseAcafTable(int year, DataClasses1DataContext zavindb)
        {
            var isPrevious = year == DateTime.Now.Year - 1;

            var date = year + "-01-01T00:00:00Z";
            var days = DateTime.Parse(date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            int toCount;
            var acafTonList = new List<KeyValuePair<string, int>>();
            switch (days)
            {
                case "ma":
                    toCount = 6;
                    break;
                case "di":
                    toCount = 5;
                    break;
                case "wo":
                    toCount = 4;
                    break;
                case "do":
                    toCount = 3;
                    break;
                case "vr":
                    toCount = 2;
                    break;
                case "za":
                    toCount = 1;
                    break;
                case "zo":
                    toCount = 0;
                    break;
                default:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            var startdate = DateTime.Parse(year + "-01-01T00:00:00Z");
            var enddate = DateTime.Parse(year + "-01-0" + (toCount + 1) + "T00:00:00Z");

            var acafTableOddWeek = from acaf in zavindb.acafs where acaf.acaf_datum >= startdate && acaf.acaf_datum <= enddate select new { date = acaf.acaf_datum, gewge = acaf.acaf_gewge };
            var total = 0;
            foreach (var acafListingOddWeek in acafTableOddWeek)
            {
                if(acafListingOddWeek.gewge != null)
                {
                    total += (int)acafListingOddWeek.gewge;
                }
            }
            acafTonList.Add(new KeyValuePair<string, int> ( "53", (total/1000)));

            var weekCounter = 0;
            var Continue = true;
            while (((startdate.Year == DateTime.Now.Year && isPrevious == false) || (startdate.Year == DateTime.Now.Year - 1 && isPrevious)) && Continue)
            {
                total = 0;
                weekCounter += 1;
                startdate = (enddate).AddHours(1);
                enddate = (enddate).AddDays(7);

                if ((enddate.Year != DateTime.Now.Year && isPrevious == false) || (enddate.Year != DateTime.Now.Year - 1 && isPrevious))
                {
                    enddate = DateTime.Parse(year + "-12-31T00:00:00Z");
                    Continue = false;
                }

                var acafTable = from acaf in zavindb.acafs where acaf.acaf_datum >= startdate && acaf.acaf_datum <= enddate select new { date = acaf.acaf_datum, gewge = acaf.acaf_gewge };
                
                foreach(var acafListing in acafTable)
                {
                    if(acafListing.gewge != null)
                    {
                        total += (int)acafListing.gewge;
                    }
                }

                acafTonList.Add(new KeyValuePair<string, int>(weekCounter.ToString(), (total/1000)));
            }

            return acafTonList;
        }

        public List<ProductionDataModel> GetProductionTable(int year)
        {
            var zavindb = new DataClasses1DataContext();

            var weekProductionTon = ParseProductionTable(zavindb, year);

            return weekProductionTon;
        }

        public List<KeyValuePair<string, int>> GetAcafTable(int year)
        {
            var zavindb = new DataClasses1DataContext();

            var weekProductionTon = ParseAcafTable(year, zavindb);

            return weekProductionTon;
        }

        public int GetPieProduction()
        {
            var zavindb = new DataClasses1DataContext();
            var year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            var startdate = DateTime.Parse(year + "-01-01T00:00:00Z");
            var enddate = DateTime.Parse(year + "-12-31T00:00:00Z");
            var total = 0;

            var yearproduction = from production in zavindb.wachtboeks where production.wb_date >= startdate && production.wb_date <= enddate select new { verstoo = production.wb_verstoo, date = production.wb_date };
            
            foreach (var datapoint in yearproduction)
            {
                if (datapoint.verstoo != null)
                {
                    total += (int)datapoint.verstoo;
                }
            }

            total /= 1000;

            return total;
        }

        private static int GetPieTarget()
        {
            var zavindb = new DataClasses1DataContext();

            var yearTarget = from production in zavindb.configs select production.YearTargetTon;
            var target = 0;
            // ReSharper disable once InconsistentNaming
            foreach (var Target in yearTarget)
            {
                if (Target != null) target = Target.Value;
            }

            return target;        
        }

        public List<KeyValuePair<string, int>> ParsePieData()
        {
            var production = GetPieProduction();
            var target = GetPieTarget();
            int calculatedProduction;
            var calculatedTarget = 0;
            var calculatedExtra = 0;
            var dataList = new List<KeyValuePair<string, int>>();

            if (production < target)
            {
                calculatedProduction = production;
                calculatedTarget = target - production;
            }
            else if (production > target)
            {
                calculatedExtra = production - target;
                calculatedProduction = target - calculatedExtra;
            }
            else
            {
                calculatedProduction = production;
            }

            dataList.Add(new KeyValuePair<string, int>("Begroting", calculatedTarget));
            dataList.Add(new KeyValuePair<string, int>("Productie", calculatedProduction));
            dataList.Add(new KeyValuePair<string, int>("Extra", calculatedExtra));

            return dataList;
        }

        public double GetWeekTarget()
        {
            var zavindb = new DataClasses1DataContext();

            var yeartarget = from target in zavindb.configs where target.Id == 1 select new { year = target.YearTargetTon };

            double weekTarget = 0;
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var date1 = new DateTime(Convert.ToInt32(DateTime.Now.Year), 12, 31);
            var cal = dfi?.Calendar;
            if (cal == null) return weekTarget;
            var weeksInYear = cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            foreach (var target in yeartarget)
            {
                weekTarget = (double)target.year / weeksInYear;
            }

            return weekTarget;
        }

        public List<KeyValuePair<string, int>> GetLineGraph()
        {
            var zavindb = new DataClasses1DataContext();

            var lineGraphData = ParseLineData(zavindb);

            return lineGraphData;
        }

        private List<KeyValuePair<string, int>> ParseLineData(DataClasses1DataContext zavindb)
        {
            var year = Convert.ToInt32(DateTime.Now.Year);
            var date = year + "-01-01T00:00:00Z";
            var days = DateTime.Parse(date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            var weekTarget = (decimal)GetWeekTarget();
            int toCount;
            var lineListTon = new List<KeyValuePair<string, int>>();
            switch (days)
            {
                case "ma":
                    toCount = 6;
                    break;
                case "di":
                    toCount = 5;
                    break;
                case "wo":
                    toCount = 4;
                    break;
                case "do":
                    toCount = 3;
                    break;
                case "vr":
                    toCount = 2;
                    break;
                case "za":
                    toCount = 1;
                    break;
                case "zo":
                    toCount = 0;
                    break;
                default:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            var startdate = DateTime.Parse(year + "-01-01T00:00:00Z");
            var enddate = DateTime.Parse(year + "-01-0" + (toCount + 1) + "T00:00:00Z");

            var lineDataOddWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate && production.wb_date <= enddate select new { verstoo = production.wb_verstoo };
            decimal total = 0;
            var oddWeekTarget = weekTarget / 7 * (toCount + 1);
            var currentWeekYear = 0;
            foreach(var lineListingOddWeek in lineDataOddWeek)
            {
                if(lineListingOddWeek.verstoo != null)
                {
                    total += (decimal)lineListingOddWeek.verstoo;
                }
            }
            var lastWeek = (total / 1000) - oddWeekTarget;
            lineListTon.Add(new KeyValuePair<string, int>("53", (int)Math.Round(lastWeek)));

            var weekCounter = 0;
            var Continue = true;

            while(startdate.Year == DateTime.Now.Year && Continue)
            {
                weekCounter++;
                total = 0;
                startdate = (enddate).AddHours(1);
                enddate = (enddate).AddDays(7);

                if (enddate > DateTime.Now)
                {
                    enddate = DateTime.Now;
                    currentWeekYear = GetCurrentWeek(enddate);
                    Continue = false;
                }

                var lineDataWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate && production.wb_date <= enddate select new { verstoo = production.wb_verstoo };

                foreach (var lineListingWeek in lineDataWeek)
                {
                    if(lineListingWeek.verstoo != null)
                    {
                        total += (decimal)lineListingWeek.verstoo;
                    }
                }

                lastWeek = lastWeek + ((total / 1000) - weekTarget);
                lineListTon.Add(new KeyValuePair<string, int>(weekCounter.ToString(), (int)Math.Round(lastWeek)));
            }

            if (currentWeekYear < 52)
            {
                var weekstofill = 52 - currentWeekYear;
                for (var i = 0; i <= weekstofill; i++)
                {
                    weekCounter++;
                    lineListTon.Add(new KeyValuePair<string, int>(weekCounter.ToString(), 0));
                }
            }

            return lineListTon;
        }

        //safe
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static int GetCurrentWeek(DateTime date)
        {
            var currentDate = date;

            var dfi = DateTimeFormatInfo.CurrentInfo;
            var cal = dfi?.Calendar;

            return cal.GetWeekOfYear(currentDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public int GetSlideTimerSeconds()
        {
            var zavindb = new DataClasses1DataContext();

            var slideTimerResult = from config in zavindb.configs select new { Timer = config.SlideTimerSeconds };
            var result = 30;

            foreach (var slideTimer in slideTimerResult)
            {
                if (slideTimer.Timer != null && slideTimer.Timer != 0)
                {
                    result = (int)slideTimer.Timer;
                }
            }

            return result;
        }

        public int GetMemoConfig()
        {
            var zavindb = new DataClasses1DataContext();

            var memoCountResult = from config in zavindb.configs select new { Count = config.MemoRunCounter };
            var result = 5;

            foreach (var memoConfig in memoCountResult)
            {
                if(memoConfig.Count != null && memoConfig.Count != 0)
                {
                    result = (int)memoConfig.Count;
                }
            }

            return result;          
        }
    }
}