using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MODELS
{
    public class Account
    {
        
		public string? id {get;set;}
		public string? email {get;set;}
		public string? firstname {get;set;}
		public string? lastname {get;set;}
		public string? username {get;set;}
		public string? password {get;set;}
		public bool verified {get;set;}
		public DateTime added {get;set;}
		public DateTime updated {get;set;}
    }
}