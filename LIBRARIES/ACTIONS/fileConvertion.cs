using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Configuration;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
//using Microsoft.AspNetCore.Http.Internal;
using static ACTIONS.all.msactions;

namespace ACTIONS.convert
{


    public partial class msactions : IFormConversions
    {
        public msactions()
		{
		}

        private static ConvertTypes convert;
        public ConvertTypes convertTypes
        {
            get
            {
                if (convert == null)
                    convert = new ConvertTypes();
                return convert;
            }
        }
        public ConvertTypes GetConvertTypes()
        {
            return convertTypes;
        }

        public IFormFile ConvertToFormFile(FileStreamResult result)
        {
            var ms = new MemoryStream();
            try
            {
                result.FileStream.CopyTo(ms);
                return new FormFile(baseStream: ms, baseStreamOffset: 0, length: ms.Length, result.ContentType, result.FileDownloadName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ms.Dispose();
                throw;
            }
            finally
            {
                ms.Dispose();
            }
        }


        public void SaveImageToDir(IFormFile sender, string webrootpath)
		{
			Path.GetFileName(sender.FileName);


			try
			{
                if (!Directory.Exists(webrootpath + "/Uploads/Images"))
                {
                    Directory.CreateDirectory(webrootpath + "/Uploads/Images/");
                }
                using (FileStream filestream = System.IO.File.Create(webrootpath + "/Uploads/Images/" + sender.FileName))
                {
                    sender.CopyTo(filestream);
                    filestream.Flush();
                }
            }
			catch (Exception msg)
			{
				Console.WriteLine(msg);
			}
		}


	}
}

