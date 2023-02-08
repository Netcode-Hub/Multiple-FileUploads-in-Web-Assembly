using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ORMExplained.Server.Models;
using System.Net;

namespace ORMExplained.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        public FileController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost]
        public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
        {
            List<UploadResult> uploadResults = new();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;

                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(env.ContentRootPath, "UploadsFolder", trustedFileNameForFileStorage);

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);

                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                uploadResults.Add(uploadResult);
            }
            return Ok(uploadResults);
        }
    }
}
