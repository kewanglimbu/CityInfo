using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FileController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
        }

        [HttpGet("{fileId}")]
        public ActionResult DownloadFile(string fileId)
        {
            var filePath = Path.Combine("BooksDownload", "Enterprise-Application-Patterns-Using-.NET-MAUI.pdf");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file.Length == 0 || file.Length> 20000000 || file.ContentType!= "application/pdf" )
            {
                return BadRequest("No file Content or invalid file");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), $"Uploaded_file_{ Guid.NewGuid() }.pdf");
            
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Successfully file uploaded!");
        }

    }
}
