using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zavin.Slideshow.Configuration
{
    class ConfigurationController
    {
        DatabaseController db = new DatabaseController();
        public int GetSlideCount()
        {
            var result = db.GetSlideConfig();
            return result;
        }

        public int GetYearTarget()
        {
            var result = db.GetYearTargetConfig();
            return result;
        }

        public int GetMemoCount()
        {
            var result = db.GetMemoConfig();
            return result;
        }

        public bool SetSlideCount(int slideCount)
        {
            var result = db.SetSlideConfig(slideCount);
            return result;
        }

        public bool SetYearTarget(int yearTarget)
        {
            var result = db.SetYearTargetConfig(yearTarget);
            return result;
        }

        public bool SetMemoCount(int memoCount)
        {
            var result = db.SetMemoConfig(memoCount);
            return result;
        }
    }
}
