using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC
{
    public class AccountLogic : BASELOGICCLASS, IAccountLogic
    {
        public AccountLogic(actions.Imsactions a, repo.EMAIL.IEMAIL e, repo.IAccount_REPO r, Microsoft.Extensions.Configuration.IConfiguration c) :base(a,e, r, c) {
        }
        

        public async Task<models.Account?> Login(string? email, string? password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                return await this.user.Login(email, password);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteAccount(string email){
            return await this.user.DeleteAccount(email);
        }

        public async Task<bool> CreateAccount(
            string? email,
            string? password,
            string? firstname,
            string? lastname,
            string? username
        )
        {
            if (
                !string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(password)
                && !string.IsNullOrEmpty(firstname)
                && !string.IsNullOrEmpty(lastname)
                && !string.IsNullOrEmpty(username)
            )
            {
                bool saved =  await this.user.CreateAccount(
                    email,
                    password,
                    firstname,
                    lastname,
                    username
                );


                int code = this.email.GetCode();
                await this.email.CreateConfirmation(new object[] { email, false, code });
                if (saved) {
                    var o = new REPO.EMAIL.EMAILMESSAGE();
                    var e = Uri.EscapeDataString(actions.v2_Encrypt(email));
                    o.msg = new string[2] {$"Welcome to the Market Alert GANG {username.ToUpper()}!🔥.", $"To get the most out of your account, you must verify it by using this code: {code} on our platform found here: {api_fe}/verify/:{e}/:verify"};
                    await this.email.SendEmailMessage(email, o);
                    return saved;
                }
                else {
                    return saved;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> VerifyAccount(
            string email,
            int code
        )
        {
            return await this.user.VerifyAccount(email, code);
        }

        public async Task<models.Account?> UpdateAccount(
            string? email,
            string? new_email,
            string? password,
            string? username,
            bool? verified
        )
        {
            if (
                !string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(new_email)
                && !string.IsNullOrEmpty(password)
                && !string.IsNullOrEmpty(username)
                && verified != null
            )
            {
                if(await this.user.UpdateAccount(
                    email:email,
                    new_email:new_email,
                    password:password,
                    username:username,
                    verified:verified
                )) {
                    return await this.user.GetAccount(email);
                }
                else {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CheckEmail(string? email)
        {
            if(!string.IsNullOrEmpty(email)){
                return await this.user.CheckEmail(email);
            }else{
                return false;
            }
        }

        public async Task<bool> CheckUsername(string? username)
        {
            if(!string.IsNullOrEmpty(username)){
                return await this.user.CheckUsername(username);
            }else{
                return false;
            }
        }
    }

    public interface IAccountLogic
    {
        Task<models.Account?> Login(string? email, string? password);
        Task<bool> DeleteAccount(string email);
        Task<bool> CreateAccount(
            string? email,
            string? password,
            string? firstname,
            string? lastname,
            string? username
        );
        Task<bool> CheckEmail(string? email);
        Task<bool> CheckUsername(string? username);
        Task<models.Account?> UpdateAccount(
            string? email,
            string? new_email,
            string? password,
            string? username,
            bool? verified
        );
        Task<bool> VerifyAccount(
            string email,
            int code
        );
    }
}
