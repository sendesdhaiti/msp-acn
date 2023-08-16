using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
namespace REPO
{
    public abstract class BASEREPOCLASS
    {
        internal actions.Logging logger = new actions.Logging();
        internal readonly string? connStr;
        internal readonly actions.Imsactions action;
        internal readonly converts.IFormConversions conversions;
        internal readonly Microsoft.Extensions.Configuration.IConfiguration config;
        internal readonly string APIURL;
        public BASEREPOCLASS(actions.Imsactions a, converts.IFormConversions c, Microsoft.Extensions.Configuration.IConfiguration config_){
            action = a;
            conversions = c;
            config = config_;
            connStr = config["ConnectionStrings:Cockroachdb_string"];
            APIURL = config["ConnectionStrings:SimpleStoreFE"] ?? "No api url found";
        }
    }
}