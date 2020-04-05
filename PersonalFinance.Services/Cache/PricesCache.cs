using System;

namespace PersonalFinance.Services.Cache
{
    public static class PricesCache
    {
        public static DateTime LastGetDate { get; set; }
        public static string PricesJSON { get; set; }
    }
}
