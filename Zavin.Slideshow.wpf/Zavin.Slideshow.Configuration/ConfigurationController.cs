﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zavin.Slideshow.Configuration
{
    class ConfigurationController
    {
        DatabaseController _db = new DatabaseController();
        public int GetSlideCount()
        {
            var result = _db.GetSlideConfig();
            return result;
        }

        public int GetYearTarget()
        {
            var result = _db.GetYearTargetConfig();
            return result;
        }

        public int GetMemoCount()
        {
            var result = _db.GetMemoConfig();
            return result;
        }

        public bool SetSlideCount(int slideCount)
        {
            var result = _db.SetSlideConfig(slideCount);
            return result;
        }

        public bool SetYearTarget(int yearTarget)
        {
            var result = _db.SetYearTargetConfig(yearTarget);
            return result;
        }

        public bool SetMemoCount(int memoCount)
        {
            var result = _db.SetMemoConfig(memoCount);
            return result;
        }
    }
}
