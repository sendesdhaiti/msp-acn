using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SimpleStore_Main.Controllers
{
    [EnableCors("MyAllowAllOrigins")]
    [ApiController]
    [Route("api/[controller]")]
    public class Contact : ControllerBase
    {
        private readonly logic.EMAIL.IEMAIL_LOGIC logic;
        private readonly ims actions;
        public Contact(logic.EMAIL.IEMAIL_LOGIC l, ims a)
        {
            this.logic = l;
            this.actions = a;
        }
        private readonly string[] msg_bi_1 = new string[] { @"
            Below you will find a link to our tuesday, thursday, saturday, and sunday night business trainings held at 3pm, 7pm, and 9pm est. Are you in a different timezone? Check out the confirmation link for more meeting times and events best suited for your schedule. 
        " };








        [HttpPost("send-email-message")]
        public async Task<IActionResult> Send([FromBody] models.ContactClass o)
        {

            var sent = await this.logic.SendACNEmail(creds: new string[] { o.email ?? " no email provided ", o.fn ?? " no first name ", o.ln ?? " no last name ", o.note ?? " no note " }, msg: msg_bi_1);
            if (sent)
            {
                return Ok(new object[] { sent, "Your interest about our business to make passive income in services that tie into the realestate market has been sent successfully. YOU ARE ON YOUR WAY TO MAKING MONEY WORK FOR YOU!", });
            }
            else
            {
                return Ok(new object[] { sent, "Your interest about our business to make passive income in services that tie into the realestate market could not be sent. TRY AGAIN!", });
            }
        }

        [HttpPut("update-email-confirmation")]
        public async Task<IActionResult> Confirm([FromBody] models.MeetingConfirmation o)
        {
            models.confirmation confirmation;
            string e;
            try
            {
                e = actions.v2_Decrypt(o.email);
            }
            catch {
                e = "";
            }

            if (o.confirmation == true)
            {
                confirmation = models.confirmation.Confirmed;
            }
            else
            {
                confirmation = models.confirmation.Not_Going;
            }

            object[] send;
            if (o.date != null)
            {
                send = new object[] { e, (int)confirmation, o.code, o.date };
            }
            else
            {
                send = new object[] { e, (int)confirmation, o.code };
            }

            var sent = await this.logic.UpdateConfirmation(creds: send);
            if (sent)
            {
                return Ok(new object[] { sent, $"Your interest about our business meetings where you can learn to make passive income with a team in services has been officially updated. CHECK YOUR EMAIL AT '{e.ToUpper()}' FOR MORE DETAILS! If you chose to decline the confirmation, you may ignore this message.", });
            }
            else
            {
                return Ok(new object[] { sent, "Your interest about our business could not be completed. TRY AGAIN LATER!", });
            }
        }

        [HttpGet("get-email-confirmations")]
        public async Task<IActionResult> GetConfirmations([FromQuery] string encryptedUser)
        {
            string e = actions.DecryptFromClient(encryptedUser);
            bool all_or_personal = false;
            if (e == "sendes12@gmail.com" || e == "sdhaiti.business@gmail.com")
            {
                all_or_personal = true;
            }
            return Ok(await this.logic.GetConfirmations(new object[] { e }, all_or_personal));
        }

        [HttpGet("check-email-notifications")]
        public async Task<IActionResult> CheckConfirmation([FromQuery] string encryptedUser)
        {
            return Ok(await this.logic.CheckConfirmation(actions.v2_Decrypt(encryptedUser)));
        }

        public class _meetingTimes
        {
            public string? email { get; set; }
            public models.MeetingTime[]? times { get; set; }
        }

        [HttpPost("add-meeting-time")]
        public async Task<IActionResult> CreateMeetingTimes([FromBody] _meetingTimes o)
        {
            if (o.email != null && o.times != null)
            {
                string e;
                try
                {
                    e = actions.v2_Decrypt(o.email);

                }
                catch
                {
                    e = "unencrypted email was used";
                }
                int check = await this.logic.CreateMeetingTime(e, o.times);
                var x = new object[] { };
                if (check > 1)
                {
                    x.Append(true);
                    x.Append("The meeting times are now live.");
                }
                else
                {
                    x.Append(false);
                    x.Append("The meeting times could not be uploaded.");
                }
                return Ok(x);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("get-meeting-times")]
        public async Task<IActionResult> GetMeetingTimes(string encryptedUser)
        {
            var e = actions.v2_Decrypt(encryptedUser);
            if (!actions.IsValidEmail(e))
            {
                return Ok(null);
            }
            return Ok(await this.logic.GetMeetingTimes());
        }
    }
}