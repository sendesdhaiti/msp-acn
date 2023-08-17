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
        public Contact(logic.EMAIL.IEMAIL_LOGIC l)
        {
            this.logic = l;
        }
        private readonly string[] msg_bi_1 = new string[] { @"
            Below you will find a link to our tuesday, thursday, saturday, and sunday night business trainings held at 3pm and 7pm est.
        " };


        public class ContactClass
        {
            public string? email { get; set; }
            public string? fn { get; set; }
            public string? ln { get; set; }
            public string? note { get; set; }
        }

        public class MeetingConfirmation
        {
            public string? email { get; set; }
            public bool confirmation { get; set; }
            public int code { get; set; }
        }

        enum confirmation
        {
            No_Action,
            Not_Going,
            Confirmed
        }

        [HttpPost("send-email-message")]
        public async Task<IActionResult> Send([FromBody] ContactClass o)
        {

            var sent = await this.logic.SendEmail(creds: new string[] { o.email ?? " no email provided ", o.fn ?? " no first name ", o.ln ?? " no last name ", o.note ?? " no note " }, msg: msg_bi_1);
            if (sent)
            {
                return Ok(new object[] { sent, "Your interest about our business to make passive income in services that tie into the realestate market has been sent successfully. <span><b>YOU ARE ON YOUR WAY TO MAKING MONEY WORK FOR YOU!</b></span>", });
            }
            else
            {
                return Ok(new object[] { sent, "Your interest about our business to make passive income in services that tie into the realestate market could not be sent. <span><b>TRY AGAIN!</b></span>", });
            }
        }

        [HttpPut("confirm-email-message")]
        public async Task<IActionResult> Confirm([FromBody] MeetingConfirmation o)
        {
            confirmation confirmation;

            if (o.confirmation == true)
            {
                confirmation = confirmation.Confirmed;
            }
            else
            {
                confirmation = confirmation.Not_Going;
            }

            var sent = await this.logic.CreateConfirmation(creds: new object[] { o.email ?? " no email provided ", confirmation, o.code });
            if (sent)
            {
                return Ok(new object[] { sent, "Your interest about our business to make passive income in services that tie into the realestate market has been officially confirmed as ATTENDING. <span><b>SEE YOU THERE!</b></span>", });
            }
            else
            {
                return Ok(new object[] { sent, "Your interest about our business to make passive income in services that tie into the realestate market has been officially confirmed as NOT GOING. <span><b>SORRY TO SEE YOU GO SO SOON. WE HOPE TO SEE YOU NEXT TIME THOUGH!</b></span>", });
            }
        }
    }
}