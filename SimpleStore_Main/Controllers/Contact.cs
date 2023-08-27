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
                if (o.v2_or_client == true)
                {
                    e = actions.v2_Decrypt(o.email);
                }
                else
                {

                    e = actions.DecryptFromClient(o.email);
                }
            }
            catch
            {
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
                send = new object[4] { e, (int)confirmation, o.code, o.date };
            }
            else
            {
                send = new object[3] { e, (int)confirmation, o.code };
            }



            var sent = await this.logic.UpdateConfirmation(creds: send, o.time);
            if (sent)
            {
                return Ok(new object[] { sent, $"Your interest about our business meetings where you can learn to make passive income with a team in services has been officially updated. CHECK YOUR EMAIL AT '{e.ToUpper()}' FOR MORE DETAILS! If you chose to decline the confirmation, you may ignore this message.", });
            }
            else
            {
                return Ok(new object[] { sent, "Your interest about our business could not be completed. TRY AGAIN LATER!", });
            }
        }

        [HttpPut("new-email-confirmation-code")]
        public async Task<IActionResult> NewConfirmationCode([FromQuery] GetMeetingTimesClass o)
        {
            string e;
            try
            {
                if (o.v2_or_client_Encryption)
                {
                    e = actions.v2_Decrypt(o.useremail);
                }
                else
                {

                    e = actions.DecryptFromClient(o.useremail);
                }
            }
            catch
            {
                e = "";
            }


            
            var sent = await this.logic.NewConfirmationCode(e);
            if (sent)
            {
                return Ok(new object[] { sent, $"Your code has been updated. Check your email at '{e.ToUpper()}' for more details.", });
            }
            else
            {
                return Ok(new object[] { sent, "Your code could not be updated. TRY AGAIN LATER!", });
            }
        }

        [HttpGet("portal/get-email-confirmations")]
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
            public string email { get; set; }
            public models.MeetingTime[] times { get; set; }
        }

        [HttpPost("portal/add-meeting-time")]
        public async Task<IActionResult> CreateMeetingTimes([FromBody] _meetingTimes meetingtime_obj)
        {
            Console.WriteLine(ACTIONS.all.msactions._ToString(meetingtime_obj));
            if (meetingtime_obj.email != null && meetingtime_obj.times != null)
            {
                string e;
                try
                {
                    e = actions.DecryptFromClient(meetingtime_obj.email);
                    for (int i = 0; i < meetingtime_obj.times.Length - 1; i++)
                    {
                        meetingtime_obj.times[i].creator = actions.DecryptFromClient(meetingtime_obj.times[i].creator);
                    }

                }
                catch
                {
                    e = "unencrypted email was used";
                }
                
                Console.WriteLine(e);
                int check = await this.logic.CreateMeetingTime(e, meetingtime_obj.times);
                var x = new object[2] { false, "" };
                Console.WriteLine(check);
                if (check > 0)
                {
                    x[0] = true;
                    x[1] = "The meeting times are now live.";
                }
                else
                {
                    x[0] = false;
                    x[1] = "The meeting times could not be uploaded.";
                }
                return Ok(x);
            }
            else
            {
                return BadRequest();
            }
        }
        public class GetMeetingTimesClass
        {
            [FromQuery(Name = "encryptedUser")]
            public string? useremail { get; set; }
            [FromQuery(Name = "v2_or_client_Encryption")]
            public bool v2_or_client_Encryption { get; set; }
        }

        [HttpGet("get-meeting-times")]
        public async Task<IActionResult> GetMeetingTimes([FromQuery] GetMeetingTimesClass user)
        {
            string? all = Request.Headers["get_all"];
            Console.WriteLine(ACTIONS.all.msactions._ToString(user));
            string e;
            if (user.v2_or_client_Encryption == true)
            {
                e = actions.v2_Decrypt(user.useremail);
            }
            else
            {
                e = actions.DecryptFromClient(user.useremail);
            }
            Console.WriteLine(ACTIONS.all.msactions._ToString(e));
            if (!actions.IsValidEmail(e))
            {
                return Ok(new object[1] { "not a valid email" });
            }
            models.MeetingTime[]? mt;
            if (all == "get_all") {
                 mt = await this.logic.GetMeetingTimes(null);
            }
            else {
                 mt = await this.logic.GetMeetingTimes(e);
            }
            Console.WriteLine(ACTIONS.all.msactions._ToString(mt));
            return Ok(mt);
        }
    }
}