using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Zavin.Slideshow.wpf
{
    class DatabaseController
    {
        private List<ProductionDataModel> ParseProductionTable(DataClasses1DataContext zavindb, int year, bool isLacal)
        {
            var isPrevious = year == DateTime.Now.Year - 1;
            var date = year + "-01-01T00:00:00Z";
            var days = DateTime.Parse(date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            int toCount;
            switch (days)
            {
                case "ma":
                    toCount = 0;
                    break;
                case "di":
                    toCount = 1;
                    break;
                case "wo":
                    toCount = 2;
                    break;
                case "do":
                    toCount = 3;
                    break;
                case "vr":
                    toCount = 4;
                    break;
                case "za":
                    toCount = 5;
                    break;
                case "zo":
                    toCount = 6;
                    break;
                default:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            var weekProductionTon = new List<ProductionDataModel>();
            var startdate = DateTime.Parse(year + "-01-01T00:00:00");
            var enddate = DateTime.Parse(year + "-01-0" + (toCount + 1) + "T00:00:00");

            if (startdate == enddate)
            {
                goto noweek;
            }

            var startdate1 = startdate;
            var enddate1 = enddate;
            var productionTableOddWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate1 && production.wb_date <= enddate1 select new { verstoo = production.wb_verstoo, wasta = production.wb_wasta, lacal = production.wb_local};
            var total = 0;
            var wasta = 0;

            foreach (var productionListingOddWeek in productionTableOddWeek)
            {
                if (productionListingOddWeek.verstoo != null && !isLacal)
                {
                    total += (int)productionListingOddWeek.verstoo;
                }
                else if (productionListingOddWeek.lacal != null && isLacal)
                {
                    total += (int)productionListingOddWeek.lacal;
                }

                if (productionListingOddWeek.wasta != null && !isLacal)
                {
                    if (productionListingOddWeek.wasta is 1 && wasta != 2)
                    {
                        wasta = 1;
                    }
                    else if (productionListingOddWeek.wasta is 2)
                    {
                        wasta = 2;
                    }
                }
                else if(isLacal)
                {
                    wasta = 3;
                }
            }

            weekProductionTon.Add(new ProductionDataModel { Week = "53", Burned = total / 1000, Wasta = wasta });

            noweek:

            if (weekProductionTon.Count == 0)
            {
                weekProductionTon.Add(new ProductionDataModel() {Week = "53", Burned = 0, Wasta = 0});
            }

            var weekCounter = 0;
            var Continue = true;
            while ((startdate.Year == DateTime.Now.Year && isPrevious == false || startdate.Year == DateTime.Now.Year - 1 && isPrevious) && Continue)
            {
                total = 0;
                wasta = 0;
                weekCounter += 1;
                startdate = enddate;
                enddate = enddate.AddDays(7);

                if (enddate.Year != DateTime.Now.Year && isPrevious == false || enddate.Year != DateTime.Now.Year -1 && isPrevious)
                {
                    enddate = DateTime.Parse(year + "-12-31T10:00:00Z");
                    Continue = false;
                }

                var startdate2 = startdate;
                var enddate2 = enddate;
                var productionTableWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate2 && production.wb_date <= enddate2 select new { verstoo = production.wb_verstoo, wasta = production.wb_wasta, lacal = production.wb_local };

                foreach (var productionListing in productionTableWeek)
                {
                    if (productionListing.verstoo != null && !isLacal)
                    {
                        total += (int)productionListing.verstoo;
                    }
                    else if (productionListing.lacal != null && isLacal)
                    {
                        total += (int)productionListing.lacal;
                    }

                    if (productionListing.wasta != null && !isLacal)
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
                    else if(isLacal)
                    {
                        wasta = 3;
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

        private Memo ParseMemo(int number, DataClasses1DataContext db, string type)
        {
            var date = DateTime.Now;
            var parseMemo = new Memo();
            var iterator = 1;

            if(type == "memo")
            {
                var memoTableValidMemo = from memo in db.infopers where memo.info_date <= date && memo.info_date2 >= date && memo.info_type == true select new { title = memo.info_desc, desc = memo.info_comm, creation = memo.info_date, author = memo.info_craft, image = memo.info_bitmap };

                foreach (var memoItem in memoTableValidMemo)
                {
                    if (iterator == number)
                    {
                        parseMemo.Title = memoItem.title;
                        parseMemo.Description = memoItem.desc;
                        parseMemo.Author = memoItem.author;
                        parseMemo.PostDate = (DateTime)memoItem.creation;
                        parseMemo.ImagePath = memoItem.image;
                    }

                    iterator++;
                }
            }
            else if (type == "welcome")
            {
                var memoTableValidWelcome = from memo in db.infopers where memo.info_date <= date && memo.info_date2 >= date & memo.info_type2 == true select new { title = memo.info_desc, desc = memo.info_comm, image = memo.info_bitmap };

                foreach (var memoItem in memoTableValidWelcome)
                {
                    parseMemo.Title = memoItem.title;
                    parseMemo.Description = memoItem.desc;
                    parseMemo.ImagePath = memoItem.image;
                    parseMemo.Author = "Do not show";
                    parseMemo.PostDate = DateTime.MinValue;
                }
            }
            else
            {
                throw new NotImplementedException("No other Memo type than 'memo' or 'welcome' has been implemented");
            }

            return parseMemo;
        }

        private int CountMemos(DataClasses1DataContext db)
        {
            var date = DateTime.Now;
            var memoCount = 0;

            var memotable = from memo in db.infopers where memo.info_date <= date && memo.info_date2 >= date && memo.info_type == true select memo;

            foreach(var unused in memotable)
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
            
            foreach (var unused in welcometable)
            {
                return true;
            }

            return false;
        }

        private List<KeyValuePair<string, int>> ParseAcafTable(int year, bool isLacal, DataClasses1DataContext zavindb)
        {
            bool isPrevious = year == DateTime.Now.Year - 1;

            var date = year + "-01-01T00:00:00Z";
            var days = DateTime.Parse(date).ToString("ddd", CultureInfo.CreateSpecificCulture("nl-NL"));
            int toCount;
            var acafTonList = new List<KeyValuePair<string, int>>();
            switch (days)
            {
                case "ma":
                    toCount = 0;
                    break;
                case "di":
                    toCount = 1;
                    break;
                case "wo":
                    toCount = 2;
                    break;
                case "do":
                    toCount = 3;
                    break;
                case "vr":
                    toCount = 4;
                    break;
                case "za":
                    toCount = 5;
                    break;
                case "zo":
                    toCount = 6;
                    break;
                default:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            var startdate = DateTime.Parse(year + "-01-01T00:00:00");
            var enddate = DateTime.Parse(year + "-01-0" + (toCount + 1) + "T00:00:00");

            if (startdate == enddate)
            {
                goto noweek;
            }

            var startdate1 = startdate;
            var enddate1 = enddate;
            var acafTableOddWeek = from acaf in zavindb.acafs where acaf.acaf_datum >= startdate1 && acaf.acaf_datum <= enddate1 select new { date = acaf.acaf_datum, gewge = acaf.acaf_gewge, lacal = acaf.acaf_lacal };
            var total = 0;
            foreach (var acafListingOddWeek in acafTableOddWeek)
            {
                if (acafListingOddWeek.gewge != null && !isLacal)
                {
                    total += (int)acafListingOddWeek.gewge;
                } else if(acafListingOddWeek.lacal != null && isLacal){
                    total += (int)acafListingOddWeek.lacal;
                }
            }
            acafTonList.Add(new KeyValuePair<string, int> ( "53", total/1000));

            noweek:

            if (acafTonList.Count == 0)
            {
                acafTonList.Add(new KeyValuePair<string, int>("53", 0));
            }

            var weekCounter = 0;
            var Continue = true;
            while ((startdate.Year == DateTime.Now.Year && isPrevious == false || startdate.Year == DateTime.Now.Year - 1 && isPrevious) && Continue)
            {
                total = 0;
                weekCounter += 1;
                startdate = enddate;
                enddate = enddate.AddDays(7);

                if (enddate.Year != DateTime.Now.Year && isPrevious == false || enddate.Year != DateTime.Now.Year - 1 && isPrevious)
                {
                    enddate = DateTime.Parse(year + "-12-31T00:00:00Z");
                    Continue = false;
                }

                var startdate2 = startdate;
                var enddate2 = enddate;
                var acafTable = from acaf in zavindb.acafs where acaf.acaf_datum >= startdate2 && acaf.acaf_datum <= enddate2 select new { date = acaf.acaf_datum, gewge = acaf.acaf_gewge, lacal = acaf.acaf_lacal };
                
                foreach(var acafListing in acafTable)
                {
                    if(acafListing.gewge != null && !isLacal)
                    {
                        total += (int)acafListing.gewge;
                    }
                    else if (acafListing.lacal != null && isLacal)
                    {
                        total += (int)acafListing.lacal;
                    }
                }

                acafTonList.Add(new KeyValuePair<string, int>(weekCounter.ToString(), total/1000));
            }

            return acafTonList;
        }

        public List<ProductionDataModel> GetProductionTable(int year, bool isLacal)
        {
            var zavindb = new DataClasses1DataContext();

            var weekProductionTon = ParseProductionTable(zavindb, year, isLacal);

            return weekProductionTon;
        }

        public List<KeyValuePair<string, int>> GetAcafTable(int year, bool isLacal)
        {
            var zavindb = new DataClasses1DataContext();

            var weekProductionTon = ParseAcafTable(year, isLacal, zavindb);

            return weekProductionTon;
        }

        public int GetPieProduction()
        {
            var zavindb = new DataClasses1DataContext();
            var year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            var startdate = DateTime.Parse(year + "-01-01T00:00:00");
            var enddate = DateTime.Parse(year + "-12-31T00:00:00Z");
            decimal total = 0;

            var yearproduction = from production in zavindb.wachtboeks where production.wb_date >= startdate && production.wb_date <= enddate select new { verstoo = production.wb_verstoo, date = production.wb_date };
            
            foreach (var datapoint in yearproduction)
            {
                if (datapoint.verstoo != null)
                {
                    total += (decimal)datapoint.verstoo;
                }
            }

            total /= 1000;

            return (int)Math.Round(total);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private int GetPieTarget()
        {
            var zavindb = new DataClasses1DataContext();

            var yearTarget = from production in zavindb.configs select production.YearTargetTon;
            var target = 0;
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
            if (dfi == null) return weekTarget;
            var cal = dfi.Calendar;
            var weeksInYear = cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            foreach (var target in yeartarget)
            {
                // ReSharper disable once PossibleLossOfFraction
                weekTarget = (int)target.year / weeksInYear;
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
            var weekTarget = GetWeekTarget();
            int toCount;
            var lineListTon = new List<KeyValuePair<string, int>>();
            var lineListTonSorted = new List<KeyValuePair<string, int>>();
            switch (days)
            {
                case "ma":
                    toCount = 0;
                    break;
                case "di":
                    toCount = 1;
                    break;
                case "wo":
                    toCount = 2;
                    break;
                case "do":
                    toCount = 3;
                    break;
                case "vr":
                    toCount = 4;
                    break;
                case "za":
                    toCount = 5;
                    break;
                case "zo":
                    toCount = 6;
                    break;
                default:
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Day not recognized by system, contact developers");
            }

            var startdate = DateTime.Parse(year + "-01-01T00:00:00");
            var enddate = DateTime.Parse(year + "-01-0" + (toCount + 1) + "T00:00:00");
            var lastWeek = 0.0;
            var currentWeekYear = 0;

            if (startdate == enddate)
            {
                goto noweek;
            }

            var startdate1 = startdate;
            var enddate1 = enddate;
            var lineDataOddWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate1 && production.wb_date <= enddate1 select new { verstoo = production.wb_verstoo };
            var total = 0;
            var oddWeekTarget = weekTarget / 7 * (toCount + 1);
            foreach(var lineListingOddWeek in lineDataOddWeek)
            {
                if(lineListingOddWeek.verstoo != null)
                {
                    total += (int)lineListingOddWeek.verstoo;
                }
            }
            // ReSharper disable once PossibleLossOfFraction
            lastWeek = total / 1000 - oddWeekTarget;
            lineListTon.Add(new KeyValuePair<string, int>("53", (int)lastWeek));

            noweek:

            if (lineListTon.Count == 0)
            {
                lineListTon.Add(new KeyValuePair<string, int>("53", 0));
            }

            var weekCounter = 0;

            while(startdate.Year == DateTime.Now.Year)
            {
                weekCounter++;
                total = 0;
                startdate = enddate.AddHours(1);
                enddate = enddate.AddDays(7);

                if (enddate > DateTime.Now)
                {
                    enddate = DateTime.Now;
                    currentWeekYear = GetCurrentWeek(enddate);
                    lineListTon.Add(new KeyValuePair<string, int>(weekCounter.ToString(), 0));
                    break;
                }

                var startdate2 = startdate;
                var enddate2 = enddate;
                var lineDataWeek = from production in zavindb.wachtboeks where production.wb_date >= startdate2 && production.wb_date <= enddate2 select new { verstoo = production.wb_verstoo };

                foreach (var lineListingWeek in lineDataWeek)
                {
                    if(lineListingWeek.verstoo != null)
                    {
                        total += (int)lineListingWeek.verstoo;
                    }
                }

                // ReSharper disable once PossibleLossOfFraction
                lastWeek = lastWeek + (total / 1000 - weekTarget);
                lineListTon.Add(new KeyValuePair<string, int>(weekCounter.ToString(), Convert.ToInt32(lastWeek)));
            }

            if (currentWeekYear >= 52) return lineListTon;
            var weekstofill = 52 - currentWeekYear;
            for (var i = 0; i <= weekstofill; i++)
            {
                weekCounter++;
                lineListTon.Add(new KeyValuePair<string, int>(weekCounter.ToString(), 0));
            }

//            for (int i = 0; i <= 52; i++)
//            {
//                if (i == 0)
//                {
//                    lineListTonSorted.Add(lineListTon.First(kvp => kvp.Key == "53"));
//                }
//                else
//                {
//                    lineListTonSorted.Add(lineListTon.First(kvp => kvp.Key == i.ToString()));
//                }
//            }

            return lineListTon;
        }

        //safe
        public static int GetCurrentWeek(DateTime date)
        {
            var currentDate = date;

            var dfi = DateTimeFormatInfo.CurrentInfo;
            if (dfi == null) return 0;
            var cal = dfi.Calendar;

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