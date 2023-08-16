using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleStore_Main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Contact : ControllerBase
    {
        private readonly logic.EMAIL.IEMAIL_LOGIC logic;
        public Contact(logic.EMAIL.IEMAIL_LOGIC l){
            this.logic = l;
        }
        private readonly string[] msg_bi_1 = new string[] { @"
            Below you will find a link to our tuesday, thursday, saturday, and sunday night business trainings held at 3pm and 7pm est.
        "};

        
        public class ContactClass{
            public string? email {get; set;}
            public string? fn {get; set;}
            public string? ln {get; set;}
            public string? note {get; set;}
        }
        [HttpPost("send-email-message")]
        public async Task<IActionResult> Post([FromBody] ContactClass o ){
            var sent = await this.logic.SendEmail(creds:new string[]{o.email ?? " no email provided ", o.fn ?? " no first name ", o.ln ?? " no last name ", o.note ?? " no note " }, msg:msg_bi_1);    
            if(sent){
                return Ok(new object[]{sent, "Your interest about our business to make passive income in services that tie into the realestate market has been sent successfully. <span><b>YOU ARE ON YOUR WAY TO MAKING MONEY WORK FOR YOU!</b></span>",});
            }else{
                 return Ok(new object[]{sent, "Your interest about our business to make passive income in services that tie into the realestate market could not be sent. <span><b>TRY AGAIN!</b></span>",});
            }
        }
    }
}