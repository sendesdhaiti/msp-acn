using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using static ACTIONS.all.msactions;

namespace ACTIONS.convert
{
    public interface IFormConversions {
        IFormFile ConvertToFormFile(FileStreamResult result);
        void SaveImageToDir(IFormFile sender, string webrootpath);
        ConvertTypes GetConvertTypes();
    }
}

