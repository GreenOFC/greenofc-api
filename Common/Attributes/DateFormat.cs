using System;

namespace _24hplusdotnetcore.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateFormat : Attribute
    {
        public string Format { get; set; }

        public DateFormat(string format)
        {
            Format = format;
        }
    }
}
