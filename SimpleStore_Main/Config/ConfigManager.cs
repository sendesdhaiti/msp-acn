using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore_Main
{
    public static class ConfigManager
    {
        public static IConfiguration AppSetting_Dev { get; }
        public static IConfiguration AppSetting_Prod { get; }

        static ConfigManager()
        {
            AppSetting_Dev = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.development.json")
                .Build();

            AppSetting_Prod = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json")
                .Build();
        }

        
    }
}
