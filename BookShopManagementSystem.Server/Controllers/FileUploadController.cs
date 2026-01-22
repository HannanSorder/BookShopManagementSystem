using Microsoft.AspNetCore.Mvc;

namespace BookShop.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Invalid file type. Only images are allowed." });
            }

            // Validate file size (5MB max)
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { message = "File size must be less than 5MB" });
            }

            try
            {
                // ✅ Safe web root resolve
                var webRoot = _environment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
                    Directory.CreateDirectory(webRoot);
                }

                // ✅ Correct folder case: "Images"
                var uploadsFolder = Path.Combine(webRoot, "Images", "books");
                Directory.CreateDirectory(uploadsFolder);

                // Create unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ✅ Correct URL case
                var imageUrl = $"/Images/books/{fileName}";
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error uploading file: {ex.Message}" });
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteFile([FromQuery] string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return BadRequest(new { message = "Image URL is required" });
            }

            try
            {
                var webRoot = _environment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
                    Directory.CreateDirectory(webRoot);
                }

                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(webRoot, "Images", "books", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok(new { message = "File deleted successfully" });
                }

                return NotFound(new { message = "File not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error deleting file: {ex.Message}" });
            }
        }
    }
}
