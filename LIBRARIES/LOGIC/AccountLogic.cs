using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC
{
    public class AccountLogic : BASELOGICCLASS, IAccountLogic
    {
        public AccountLogic(actions.Imsactions a, repo.IAccount_REPO acc):base(a,acc) { }

        public async Task<models.Account?> Login(string? email, string? password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                return await this.repo.Login(email, password);
            }
            else
            {
                return null;
            }
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
                return await this.repo.CreateAccount(
                    email,
                    password,
                    firstname,
                    lastname,
                    username
                );
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckEmail(string? email)
        {
            if(!string.IsNullOrEmpty(email)){
                return await this.repo.CheckEmail(email);
            }else{
                return false;
            }
        }

        public async Task<bool> CheckUsername(string? username)
        {
            if(!string.IsNullOrEmpty(username)){
                return await this.repo.CheckUsername(username);
            }else{
                return false;
            }
        }
    }

    public interface IAccountLogic
    {
        Task<models.Account?> Login(string? email, string? password);
        Task<bool> CreateAccount(
            string? email,
            string? password,
            string? firstname,
            string? lastname,
            string? username
        );
        Task<bool> CheckEmail(string? email);
        Task<bool> CheckUsername(string? username);
    }
}
