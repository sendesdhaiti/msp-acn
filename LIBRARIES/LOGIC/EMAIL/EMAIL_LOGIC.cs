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
        Task<models.MeetingTime[]> GetMeetingTimes(string? email);
        Task<models.MeetingConfirmation[]?> CheckConfirmation(string email);
        Task<bool> UpdateConfirmation(object[] creds, MeetingTime? time);
        Task<bool> NewConfirmationCode(string email);
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

        public async Task<bool> NewConfirmationCode(string email)
        {
            Console.WriteLine(email);
            int code = this.email.GetCode();
            bool sent = await this.email.NewConfirmationCode(email, code);
            if (sent)
            {

                sent = await this.email.SendEmailMessage(email, new EMAILMESSAGE() { msg = new string[1] { $"Enter this code:{code} to send a new confirmation for a meeting" } });
                return sent;
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

        public async Task<bool> UpdateConfirmation(object[] creds, MeetingTime? time)
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
                    DateTime date = (DateTime)creds[3];
                    if(time != null) {
                        var a = await this.user.GetAccount((string)creds[0]);
                        if (a != null)
                        {
                            await this.email.SendEmailMessage(time.hostemail ?? "",
                                new EMAILMESSAGE()
                                {
                                    msg = new string[1] { $"The person by the name of {a.firstname} {a.lastname} | {(string)creds[0]} has confirmed to join your meeting {time.day} at {time.time} {time.timezone} at this time: {date.ToLongDateString()} at {date.ToShortTimeString()}." }
                                }
                            );
                        }
                    }
                    var m = new EMAILMESSAGE() { };
                    string e = actions.v2_Encrypt((string)creds[0]);
                    e = Uri.EscapeDataString(e);
                    await this.user.UpdateAccount(e, null, null, null, true);
                    m.msg = new string[2] { $"Your attendance for the business meeting on {date.ToLongDateString()} at {date.ToShortTimeString()} has been confirmed.",
                        $"You can find these times and your confirmation by following this link: See Meeting Time '{api_fe}/my-confirmations/:{e}'" };
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
        public async Task<models.MeetingTime[]> GetMeetingTimes(string? email)
        {
            if (email == null)
            {
                return await this.email.GetMeetingTimes();
            }
            return await this.email.GetMeetingTimes(email);
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
