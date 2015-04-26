using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.Helpers
{
    public static class FormatHelper
    {
        public static string GetDayMonth(DateTime date)
        {
            return string.Format("{0}/{1}", date.Day, date.Month); 
        }
    }
}