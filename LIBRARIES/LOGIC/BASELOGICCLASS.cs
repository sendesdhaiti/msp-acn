using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC
{
    public abstract class BASELOGICCLASS
    {
        internal readonly actions.Imsactions actions;
        internal readonly repo.IAccount_REPO user;
        internal readonly repo.IProduct_REPO? prod;
        internal readonly repo.IOrder_REPO? order;
        internal readonly repo.EMAIL.IEMAIL? email;
        internal readonly Microsoft.Extensions.Configuration.IConfiguration? config;
        //internal readonly converts.IFormConversions conversions;
        internal readonly string? api_fe;

        /// <summary>
        /// account logic is used here
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        public BASELOGICCLASS(actions.Imsactions a, repo.IAccount_REPO r){
            actions = a;
            user = r;
            api_fe = config?["ConnectionStrings:SimpleStoreFE"] ?? "";
        }

        /// <summary>
        /// Email Logic is used here
        /// </summary>
        /// <param name="a"></param>
        /// <param name="e"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public BASELOGICCLASS(actions.Imsactions a, repo.EMAIL.IEMAIL e, repo.IAccount_REPO r, Microsoft.Extensions.Configuration.IConfiguration c)
        {
            actions = a;
            email = e;
            user = r;
            config = c;
            api_fe = config?["ConnectionStrings:SimpleStoreFE"] ?? "No api url found";
        }

        /// <summary>
        /// Product logic is used here
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        public BASELOGICCLASS(actions.Imsactions a, repo.IAccount_REPO r, repo.IProduct_REPO p){
            actions = a;
            user = r;
            prod = p;
        }

        /// <summary>
        /// Order logic is used here
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        /// <param name="o"></param>
        public BASELOGICCLASS(actions.Imsactions a, repo.IAccount_REPO r, repo.IProduct_REPO p, repo.IOrder_REPO o){
            actions = a;
            user = r;
            prod = p;
            order = o;
        }
    }
}