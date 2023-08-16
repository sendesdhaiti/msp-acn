using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleStore_Main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Authentication : ControllerBase
    {
        private readonly actions.Imsactions actions;
        private readonly logic.IAccountLogic accounts;

        public Authentication(actions.Imsactions a, logic.IAccountLogic acc)
        {
            actions = a;
            accounts = acc;
        }

        //TODO - AFTER MVP's
        /// <summary>
        /// User enters site and server registers their ip and location for furth analytics
        /// </summary>
        /// <returns></returns>
        [HttpGet("welcome")]
        public void WelcomeUser() { }

        /// <summary>
        /// User enters site and server registers their
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(string email, string password)
        {
            var request = Request.Headers["request"];
            if (!string.IsNullOrEmpty(request))
            {
                var account = this.actions.ConvertJsonObj<models.Account>(request);
                if (account != null 
                && account?.email != null
                && account?.password != null)
                {
                    var e = this.actions.DecryptFromClient(account.email);
                    var p = this.actions.DecryptFromClient(account.password);
                    return Ok(await this.accounts.Login(e, p));
                }
                else
                {
                    return BadRequest(null);
                }
            }
            else
            {
                var e = this.actions.DecryptFromClient(email);
                var p = this.actions.DecryptFromClient(password);
                return Ok(await this.accounts.Login(e, p));
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<object>> Register(
            string? email,
            string? password,
            string? firstname,
            string? lastname,
            string? username
        )
        {
            var request = Request.Headers["request"];
            if (!string.IsNullOrEmpty(request))
            {
                var account = this.actions.ConvertJsonObj<models.Account>(request);
                if (account != null
                && account.email != null
                && account.password != null
                && account.firstname != null
                && account.lastname != null
                && account.username != null)
                {
                    var e = this.actions.DecryptFromClient(account.email);
                    var p = this.actions.DecryptFromClient(account.password);
                    var f = this.actions.DecryptFromClient(account.firstname);
                    var l = this.actions.DecryptFromClient(account.lastname);
                    var u = this.actions.DecryptFromClient(account.username);
                    return Created("registered", await this.accounts.CreateAccount(e, p, f, l, u));
                }
                else
                {
                    return BadRequest(null);
                }
            }
            else
            {
                var e = this.actions.DecryptFromClient(email);
                var p = this.actions.DecryptFromClient(password);
                var f = this.actions.DecryptFromClient(firstname);
                var l = this.actions.DecryptFromClient(lastname);
                var u = this.actions.DecryptFromClient(username);
                return Created("registered", await this.accounts.CreateAccount(e, p, f, l, u));
            }
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<object>> CheckEmail(string? email)
        {
            var request = Request.Headers["email"];
            if (!string.IsNullOrEmpty(request))
            {
                // var account = this.actions.ConvertJsonObj<models.Account>(request);
                var e = this.actions.DecryptFromClient(request);
                return Ok(await this.accounts.CheckEmail(e));
            }
            else
            {
                var e = this.actions.DecryptFromClient(email);
                return Ok(await this.accounts.CheckEmail(e));
            }
        }

        [HttpGet("check-username")]
        public async Task<ActionResult<object>> CheckUsername(string? username)
        {
            var request = Request.Headers["username"];
            if (!string.IsNullOrEmpty(request))
            {
                // var account = this.actions.ConvertJsonObj<models.Account>(request);
                var e = this.actions.DecryptFromClient(request);
                return Ok(await this.accounts.CheckUsername(e));
            }
            else
            {
                var e = this.actions.DecryptFromClient(username);
                return Ok(await this.accounts.CheckUsername(e));
            }
        }
    }
}
