using System;

namespace _24hplusdotnetcore.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class KeyAuditingAttribute : Attribute
    {
    }
}
