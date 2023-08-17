using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REPO.EMAIL
{
    public interface IEMAIL
    {
        Task<bool> SendACNBusinessInterestMessage(
            string receiverEmail,
            string receiverFirstName,
            string receiverLastName,
            string[] msgs,
            string? note
        );
        Task<bool> RegisterUser(string[] creds);
        Task<bool> CreateConfirmation(object[] creds);
    }
}