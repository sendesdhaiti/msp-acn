using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC
{
    public abstract class BASELOGICCLASS
    {
        internal readonly actions.Imsactions actions;
        internal readonly repo.IAccount_REPO repo;
        internal readonly repo.IProduct_REPO? prod;
        internal readonly repo.IOrder_REPO? order;
        public BASELOGICCLASS(actions.Imsactions a, repo.IAccount_REPO r){
            actions = a;
            repo = r;
        }

        public BASELOGICCLASS(actions.Imsactions a, repo.IAccount_REPO r, repo.IProduct_REPO p){
            actions = a;
            repo = r;
            prod = p;
        }

        public BASELOGICCLASS(actions.Imsactions a, repo.IAccount_REPO r, repo.IProduct_REPO p, repo.IOrder_REPO o){
            actions = a;
            repo = r;
            prod = p;
            order = o;
        }
    }
}