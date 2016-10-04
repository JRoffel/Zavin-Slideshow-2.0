using System;
using System.Linq;

namespace Zavin.Slideshow.Configuration
{
    class DatabaseController
    {
        private DatabaseClassesDataContext Zavindb = new DatabaseClassesDataContext();

        internal int GetSlideConfig()
        {
            var SlideConfigResult = from config in Zavindb.configs select new { Slide = config.SlideTimerSeconds };
            var result = 0;

            foreach (var SlideConfig in SlideConfigResult)
            {
                if (SlideConfig.Slide != null)
                {
                    result = (int)SlideConfig.Slide;
                }
            }

            return result;
        }

        internal int GetYearTargetConfig()
        {
            var YearTargetResult = from config in Zavindb.configs select new { Target = config.YearTargetTon };
            var result = 0;

            foreach (var YearTargetConfig in YearTargetResult)
            {
                if(YearTargetConfig.Target != null)
                {
                    result = (int)YearTargetConfig.Target;
                }
            }

            return result;
        }

        internal int GetMemoConfig()
        {
            var MemoCountResult = from config in Zavindb.configs select new { Memo = config.MemoRunCounter };
            var result = 0;

            foreach (var MemoCountConfig in MemoCountResult)
            {
                if (MemoCountConfig.Memo != null)
                {
                    result = (int)MemoCountConfig.Memo;
                }
            }

            return result;
        }

        internal bool SetSlideConfig(int slideCount)
        {
            var SlideTimerResult = from config in Zavindb.configs select config;

            foreach (var SlideTimerConfig in SlideTimerResult)
            {
                SlideTimerConfig.SlideTimerSeconds = slideCount;
            }

            try
            {
                Zavindb.SubmitChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        internal bool SetYearTargetConfig(int yearTarget)
        {
            var YearTargetResult = from config in Zavindb.configs select config;

            foreach(var YearTargetConfig in YearTargetResult)
            {
                YearTargetConfig.YearTargetTon = yearTarget;
            }

            try
            {
                Zavindb.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        internal bool SetMemoConfig(int memoCount)
        {
            var MemoCountResult = from config in Zavindb.configs select config;

            foreach(var MemoCountConfig in MemoCountResult)
            {
                MemoCountConfig.MemoRunCounter = memoCount;
            }

            try
            {
                Zavindb.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}