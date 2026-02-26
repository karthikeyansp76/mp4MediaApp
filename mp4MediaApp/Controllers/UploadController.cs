namespace mp4MediaApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using mp4MediaApp.Services;

    /// <summary>
    /// Defines the <see cref="UploadController" />
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        /// <summary>
        /// Defines the _videoService
        /// </summary>
        private readonly IVideoService _videoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadController"/> class.
        /// </summary>
        /// <param name="videoService">The videoService<see cref="IVideoService"/></param>
        public UploadController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        /// <summary>
        /// The Upload
        /// </summary>
        /// <param name="files">The files<see cref="List{IFormFile}"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        [RequestSizeLimit(200_000_000)]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            try
            {
                await _videoService.UploadVideosAsync(files);

                return Ok(new
                {
                    success = true,
                    message = "Files uploaded successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
