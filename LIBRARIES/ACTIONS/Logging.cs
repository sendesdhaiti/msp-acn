using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACTIONS
{
    public class Logging
    {
        public static void Log(object? actionYouAreDoing, object? ForWhatOBJ, object? idEmailOrUsersTag, string? Result) { 
            Console.WriteLine(
                ACTIONS.all.msactions._break + 
                $"{ACTIONS.all.msactions._ToString(actionYouAreDoing).ToUpper() ?? "NULL"} {ACTIONS.all.msactions._ToString(ForWhatOBJ).ToUpper() ?? "NULL"} for '{ACTIONS.all.msactions._ToString(idEmailOrUsersTag).ToUpper() ?? "NULL"}' is '{ACTIONS.all.msactions._ToString(Result).ToUpper() ?? "NULL"}' at {DateTime.UtcNow.AddHours(-4)}" +
                ACTIONS.all.msactions._break
            );
        }
    }
}