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
        Task<int> UpdateMeetingTime(string email, models.MeetingTime[] times);
        Task<models.MeetingTime[]> GetMeetingTimes();
        Task<models.MeetingTime[]> GetMeetingTimes(string email);
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
        /// <summary>
        /// New Confirmation code Update to Confirmation
        /// </summary>
        /// <param name="creds"></param>
        /// <returns></returns>
        Task<bool> NewConfirmationCode(string email, int code);
        /// <summary>
        /// Get The meeting times of conr=firmation using confirmation id
        /// </summary>
        /// <param name="confirmationId"></param>
        /// <returns></returns>
        Task<models.MeetingTime> GetConfirmationMeetingTimes(Guid confirmationId);

        /// <summary>
        /// Create the relation between meetingtime and confirmation
        /// </summary>
        /// <param name="confirmation"></param>
        /// <param name="meetingTime"></param>
        /// <returns></returns>
        Task<bool> CreateConfirmationMeetingTimeRelation(Guid confirmation, Guid meetingTime);


    }
}