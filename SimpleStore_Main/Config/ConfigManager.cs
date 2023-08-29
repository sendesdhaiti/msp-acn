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
                    ACTIONS.Logging.Log("CHECKING VARIABLES","DEBUGGER",null, "Debug build");
                    Console.WriteLine(ACTIONS.all.msactions._ToString(Directory.GetDirectories(Environment.CurrentDirectory)));
                    AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("appsettings.development.json")
                        .Build();
#else
                    ACTIONS.Logging.Log("CHECKING VARIABLES","DEBUGGER",null, "Production build");
                    Console.WriteLine(ACTIONS.all.msactions._ToString(Directory.GetDirectories(Environment.CurrentDirectory)));
                    AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();
#endif

        }
    }
}
