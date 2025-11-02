using Refit;
using System;
using System.Linq;

namespace _24hplusdotnetcore.Extensions
{
    public static class HelperExtension
    {
        public static string GenerateCode(int numberOfCharacters)
        {
            Random _random = new Random();
            return string.Join("", Enumerable.Range(0, numberOfCharacters).Select(x => _random.Next(0, 9)));
        }

        public static string GetHttpMethodPath(Type type, string methodName)
        {
            var methodInfo = type.GetMethod(methodName);
            var hma = methodInfo.GetCustomAttributes(true).OfType<HttpMethodAttribute>().First();
            return hma.Path;
        }
    }
}
