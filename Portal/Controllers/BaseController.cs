using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Portal.Controllers
{
    public class BaseController : Controller
    {
        public string UserGuid
        {
            get
            {
                return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }

		public string UserEmail
		{
			get
			{
				return HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
			}
		}

        public async Task UploadFile(IFormFile formFile, string destinationFolder)
        {
            string filepath = destinationFolder + formFile.FileName;
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);
            if (!System.IO.File.Exists(filepath))
            {
                using (FileStream output = System.IO.File.Create(filepath))
                    await formFile.CopyToAsync(output);
            }
        }
    }
}
