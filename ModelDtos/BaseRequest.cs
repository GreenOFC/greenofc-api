using System;
using System.Globalization;

namespace _24hplusdotnetcore.ModelDtos
{
    public class BaseRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TextSearch { get; set; }
        public string TeamLead { get; set; }
        public string Asm { get; set; }
        public string PosManager { get; set; }
        public string Sale { get; set; }

        public DateTime GetFromDate()
        {
            return GetDateTime(FromDate) ?? DateTime.Now.AddDays(-30);
        }

        public DateTime GetToDate()
        {
            return GetDateTime(ToDate) ?? DateTime.Now.AddDays(1);
        }

        private DateTime? GetDateTime(string date)
        {
            string[] format = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
            if (!string.IsNullOrEmpty(date) && DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
    }
}
