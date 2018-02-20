using System;
using System.Linq;

namespace Zavin.Slideshow.Configuration
{
    class DatabaseController
    {
        private DatabaseClassesDataContext _zavindb = new DatabaseClassesDataContext();

        internal int GetSlideConfig()
        {
            var slideConfigResult = from config in _zavindb.configs select new { Slide = config.SlideTimerSeconds };
            var result = 0;

            foreach (var slideConfig in slideConfigResult)
            {
                if (slideConfig.Slide != null)
                {
                    result = (int)slideConfig.Slide;
                }
            }

            return result;
        }

        internal int GetYearTargetConfig()
        {
            var yearTargetResult = from config in _zavindb.configs select new { Target = config.YearTargetTon };
            var result = 0;

            foreach (var yearTargetConfig in yearTargetResult)
            {
                if(yearTargetConfig.Target != null)
                {
                    result = (int)yearTargetConfig.Target;
                }
            }

            return result;
        }

        internal int GetMemoConfig()
        {
            var memoCountResult = from config in _zavindb.configs select new { Memo = config.MemoRunCounter };
            var result = 0;

            foreach (var memoCountConfig in memoCountResult)
            {
                if (memoCountConfig.Memo != null)
                {
                    result = (int)memoCountConfig.Memo;
                }
            }

            return result;
        }

        internal bool SetSlideConfig(int slideCount)
        {
            var slideTimerResult = from config in _zavindb.configs select config;

            foreach (var slideTimerConfig in slideTimerResult)
            {
                slideTimerConfig.SlideTimerSeconds = slideCount;
            }

            try
            {
                _zavindb.SubmitChanges();
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
            var yearTargetResult = from config in _zavindb.configs select config;

            foreach(var yearTargetConfig in yearTargetResult)
            {
                yearTargetConfig.YearTargetTon = yearTarget;
            }

            try
            {
                _zavindb.SubmitChanges();
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
            var memoCountResult = from config in _zavindb.configs select config;

            foreach(var memoCountConfig in memoCountResult)
            {
                memoCountConfig.MemoRunCounter = memoCount;
            }

            try
            {
                _zavindb.SubmitChanges();
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