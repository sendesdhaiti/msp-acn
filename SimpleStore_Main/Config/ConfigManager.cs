using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore_Main
{
    public static class ConfigManager
    {
        public static IConfiguration AppSetting { get; }

        static ConfigManager()
        {
            #if DEBUG
                    Console.WriteLine("Debug build");
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.development.json")
                        .Build();
#else
                    Console.WriteLine("Prod build");
                    AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
#endif

        }
    }
}
