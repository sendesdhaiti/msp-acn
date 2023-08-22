using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REPO.EMAIL
{
    public interface IEMAIL
    {
        Task<bool> SendACNBusinessInterestMessage(
            string _receiverEmail,
            string _receiverFirstName,
            string _receiverLastName,
            string[] _msgs,
            string? _note,
            int _code
        );
        Task<bool> CreateConfirmation(object[] creds);
        Task<bool> UpdateConfirmation(object[] creds);
        Task<models.MeetingConfirmation[]?> GetConfirmations(object[] creds, bool All_or_Personal);
        Task<int> CreateMeetingTime(string email, models.MeetingTime[] times);
        Task<models.MeetingTime[]> GetMeetingTimes();
        Task<models.MeetingConfirmation[]?> CheckConfirmation(string email);
        Task<bool> SendEmailMessage(
            string _receiverEmail,
            EMAILMESSAGE _msgs
        );
        /// <summary>
        /// Gets a six digit integer code
        /// </summary>
        /// <returns></returns>
        int GetCode();


    }
}