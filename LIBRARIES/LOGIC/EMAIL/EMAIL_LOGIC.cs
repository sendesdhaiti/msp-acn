using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MODELS;
using REPO.EMAIL;

namespace LOGIC.EMAIL
{
    public interface IEMAIL_LOGIC
    {
        Task<bool> SendACNEmail(string[] creds, string[] msg);
        Task<bool> CreateConfirmation(object[] creds);
        Task<models.MeetingConfirmation[]?> GetConfirmations(object[] creds, bool All_or_Personal);
        Task<int> CreateMeetingTime(string email, models.MeetingTime[] times);
        Task<models.MeetingTime[]> GetMeetingTimes();
        Task<models.MeetingConfirmation[]?> CheckConfirmation(string email);
        Task<bool> UpdateConfirmation(object[] creds);
    }
    public class EMAIL_LOGIC : BASELOGICCLASS, IEMAIL_LOGIC
    {

        public EMAIL_LOGIC(actions.Imsactions a, REPO.EMAIL.IEMAIL e, repo.IAccount_REPO u, Microsoft.Extensions.Configuration.IConfiguration c) : base(a, e, u, c)
        {
        }

        

        public async Task<bool> SendACNEmail(string[] creds, string[] msg)
        {
            if (this.email == null)
            {
                return false;
            }
            if (creds.Length < 3)
            {
                return false;
            }

            int code = this.email.GetCode();
            var sent = await this.email.SendACNBusinessInterestMessage(creds[0], creds[1], creds[2], msg, creds[3], code);
            if (sent)
            {
                sent = await this.user.CreateAccount(creds[0], creds[0], creds[1], creds[2], creds[0]);
                if (sent)
                {
                    return await this.email.CreateConfirmation(new object[] { creds[0], 0, code });
                }
                else
                {
                    return await this.email.CreateConfirmation(new object[] { creds[0], 0, code });
                }


            }
            else
            {
                return sent;
            }
        }

        public async Task<bool> CreateConfirmation(object[] creds)
        {
            if (this.email == null)
            {
                return false;
            }
            if (creds.Length < 3)
            {
                return false;
            }
            else
            {
                return await this.email.CreateConfirmation(creds);
            }
        }

        public async Task<bool> UpdateConfirmation(object[] creds)
        {
            if (this.email == null)
            {
                return false;
            }
            if (creds.Length < 3)
            {
                return false;
            }
            else
            {
                if (await this.email.UpdateConfirmation(creds))
                {
                    var m = new EMAILMESSAGE() { };
                    string e = actions.v2_Encrypt(creds[0].ToString() ?? "");
                    e = Uri.EscapeDataString(e);
                    await this.user.UpdateAccount(e,null, null, null, true);
                    m.msg = new string[2] { $"Your attendance for the business meeting on {creds[3]} has been confirmed.",
                        $"You can find these times and your confirmation by following this link: See Meeting Time '{api_fe}/meetings/:{e}'" };
                    return await this.email.SendEmailMessage((string)creds[0], m);
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<int> CreateMeetingTime(string email, models.MeetingTime[] times)
        {
            if (this.email == null)
            {
                return 0;
            }
            if (times.Length < 1)
            {
                return 0;
            }
            else
            {
                return await this.email.CreateMeetingTime(email, times);
            }
        }

        public async Task<models.MeetingConfirmation[]?> GetConfirmations(object[] creds, bool All_or_Personal)
        {
            if (this.email == null)
            {
                return new MeetingConfirmation[] { };
            }
            if (creds.Length < 1)
            {
                return null;
            }
            else
            {
                return await this.email.GetConfirmations(creds, All_or_Personal);
            }
        }
        public async Task<models.MeetingTime[]> GetMeetingTimes()
        {
            if (this.email == null)
            {
                return new models.MeetingTime[] { };
            }
            return await this.email.GetMeetingTimes();
        }
        public async Task<models.MeetingConfirmation[]?> CheckConfirmation(string email)
        {
            if (this.email == null)
            {
                return new models.MeetingConfirmation[] { };
            }
            return await this.email.CheckConfirmation(email);
        }
    }
}
