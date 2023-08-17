using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REPO.EMAIL;

namespace LOGIC.EMAIL
{
    public interface IEMAIL_LOGIC
    {
        Task<bool> SendEmail(string[] creds, string[] msg);
        Task<bool> CreateConfirmation(object[] creds);
    }
    public class EMAIL_LOGIC : IEMAIL_LOGIC
    {
        private readonly REPO.EMAIL.IEMAIL email;
        private readonly repo.IAccount_REPO user;

        public EMAIL_LOGIC(REPO.EMAIL.IEMAIL e, repo.IAccount_REPO u)
        {
            email = e;
            user = u;
        }

        public async Task<bool> SendEmail(string[] creds, string[] msg)
        {
            if (creds.Length < 3)
            {
                return false;
            }

            var sent = await this.email.SendACNBusinessInterestMessage(creds[0], creds[1], creds[2], msg, creds[3]);
            if (sent)
            {
                return await this.email.RegisterUser(creds);
            }
            else
            {
                return sent;
            }
        }

        public async Task<bool> CreateConfirmation(object[] creds) {
            if (creds.Length < 3)
            {
                return false;
            }
            else {
                return await this.email.CreateConfirmation(creds);
            }

            
        }

        // public async Task<models.Account[]> GetBusinessInterestAccounts(string[] creds) {
        //     if(creds.Length > 0){
        //         if(creds[0] != "sendes12@gmail.com" || "sdhaiti.business@gmail.com"){
        //             return false;
        //         }
        //     }

        // }
    }
}
