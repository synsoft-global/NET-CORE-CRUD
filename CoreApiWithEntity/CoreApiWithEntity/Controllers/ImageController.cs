using Microsoft.AspNetCore.Mvc;
using CoreApiWithEntity.BLL.Interface;
using CoreApiWithEntity.DAL.Models;

namespace CoreApiWithEntity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImage _imageRepository;

        public ImageController(IImage imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUpload model)
        {
            try
            {
                var imageUrl = await _imageRepository.UploadImageAsync(model.ImageFile);
                return Ok(new { imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
