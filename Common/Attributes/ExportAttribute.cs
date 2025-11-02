using System;

namespace _24hplusdotnetcore.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class ExportAttribute : Attribute
    {
        public string ExportName { get; set; }
        public string Format { get; set; }
    }
}
