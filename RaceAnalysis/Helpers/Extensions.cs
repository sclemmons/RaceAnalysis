using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Helpers
{
    public static class Extensions
    {
        public static string ZeroIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? "0" : s;
        }


        public static string MaxIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? "1440" : s;  //24 hrs
        }
    }
}