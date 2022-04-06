using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace TollCalculator
{
    static class ConfigurationManager
    {
        public static IConfiguration AppSetting { get; }
        static ConfigurationManager()
        {
            string dir = @"C:\Users\User\source\repos\TollCalculator";
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetParent(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))).FullName)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}