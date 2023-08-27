using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using REPO.EMAIL;

namespace SimpleStore_Main.Controllers
{
    [EnableCors("MyAllowAllOrigins")]
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
        /// Log in the user to retrieve users credentials
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Account object</returns>
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] models.Account o)
        {
            if (o != null && o.email != null && o.password != null)
            {
                var e = this.actions.DecryptFromClient(o.email);
                var p = this.actions.DecryptFromClient(o.password);
                models.Account? acc = await this.accounts.Login(e, p);
                if (acc != null)
                {
                    acc.token = this.actions.GetToken().Generate_MINTSOUP_JWTtoken(acc.id ?? "empty id", acc.email ?? "empty email", acc.username, "Email Portal", "MSP BI");
                    return Ok(
                        new object[] { acc, $"Welcome back '{acc.username?.ToLower() ?? acc.email?.ToLower()}'!🔥" }
                    );
                }
                else
                {
                    return NotFound(
                        new object?[] { null, $"Your account was not found for '{e}'. Check your responses!" }
                    );
                }
            }
            else
            {
                return BadRequest(new object?[] { null, $"Something went wrong when looking for your account. Check your responses!" });
            }
        }
        public class UpdateAccount
        {
            public string? email { get; set; }
            public models.Account? account { get; set; }
        }

        [HttpPut("portal/update-account")]
        public async Task<ActionResult<object>> UpdateUserAccount([FromBody] UpdateAccount o)
        {
            if (o != null && o.email != null && o.account != null)
            {
                var e = this.actions.DecryptFromClient(o.email);
                var p = this.actions.DecryptFromClient(o.account.password);
                models.Account? updated = await this.accounts.UpdateAccount(e, o.account.email, p, o.account.username, o.account.verified);
                if (updated != null)
                {
                    return Ok(
                        new object[] { updated, $"Your account has been updated." }
                    );
                }
                else
                {
                    return Ok(
                        new object?[] { updated, $"Your account could not be updated." }
                    );
                }
            }
            else
            {
                return BadRequest(new object?[] { null, $"Something went wrong while updating your account. Check your responses!" });
            }
        }
        public class VerifyAccount
        {
            public string? email { get; set; }
            public int code { get; set; }
            public bool v2_or_client { get; set; }
        }

        [HttpPut("verify-account")]
        public async Task<ActionResult<object>> VerifyUserAccount([FromBody] VerifyAccount o)
        {
            Console.WriteLine(ACTIONS.all.msactions._ToString(o));
            if (o != null && o.email != null)
            {
                string e = "";
                if (o.v2_or_client)
                {
                    e = this.actions.v2_Decrypt(o.email);
                }
                else
                {
                    e = this.actions.DecryptFromClient(o.email);
                }
                bool updated = await this.accounts.VerifyAccount(e, o.code);
                if (updated)
                {
                    return Ok(
                        new object[] { updated, $"Your account has been updated." }
                    );
                }
                else
                {
                    return Ok(
                        new object?[] { updated, $"Your account could not be updated." }
                    );
                }
            }
            else
            {
                return BadRequest(new object?[] { null, $"Something went wrong while updating your account. Check your responses!" });
            }
        }

        [HttpDelete("portal/delete-account")]
        public async Task<ActionResult<object>> Delete([FromQuery] string? email)
        {
            if (email != null)
            {
                var e = this.actions.DecryptFromClient(email);
                bool del = await this.accounts.DeleteAccount(e);
                if (del)
                {
                    return Ok(
                        new object[] { del, "Your account has been removed. It's sad to see you go! 😢" }
                    );
                }
                else
                {
                    return NotFound(
                        new object[] { del, $"Your account was not found for '{e}'. Check your responses!" }
                    );
                }
            }
            else
            {
                return BadRequest(new object[] { false, "Something went wrong with your request. Check your responses!" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<object>> Register([FromBody] models.Account o)
        {

            if (
                o != null
                && o.email != null
                && o.password != null
                && o.firstname != null
                && o.lastname != null
                && o.username != null
            )
            {
                var e = this.actions.DecryptFromClient(o.email);
                var p = this.actions.DecryptFromClient(o.password);
                var f = this.actions.DecryptFromClient(o.firstname);
                var l = this.actions.DecryptFromClient(o.lastname);
                var u = this.actions.DecryptFromClient(o.username);
                bool del = await this.accounts.CreateAccount(e, p, f, l, u);
                if (del)
                {
                    return Created(
                        "new-user",
                        new object[] { del, "Your account has been created. WELCOME TO THE MARKET ALERT GANG! 🔥" }
                    );
                }
                else
                {
                    return Created(
                        "new-user",
                        new object[] { del, $"Your account was not created for '{e}'. Check your responses!" }
                    );
                }
            }
            else
            {
                return BadRequest(new object[] { false, "Something went wrong with your request. Check your responses!" });
            }
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<object>> CheckEmail([FromQuery] string? email)
        {
            var e = this.actions.DecryptFromClient(email);
            bool del = await this.accounts.CheckEmail(e);
            if (del)
            {
                return Ok(
                    new object[] { del, "Your email has been found." }
                );
            }
            else
            {
                return NotFound(
                    new object[] { del, $"'{e}' is not registered yet." }
                );
            }
        }

        [HttpGet("check-username")]
        public async Task<ActionResult<object>> CheckUsername([FromQuery] string? username)
        {
            var u = this.actions.DecryptFromClient(username);
            bool del = await this.accounts.CheckUsername(u);
            if (del)
            {
                return Ok(
                    new object[] { del, "Your username has been found." }
                );
            }
            else
            {
                return NotFound(
                    new object[] { del, $"'{u}' is not registered yet." }
                );
            }
        }
    }
}
