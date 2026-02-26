namespace mp4MediaApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using mp4MediaApp.Services;

    /// <summary>
    /// Defines the <see cref="HomeController" />
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Defines the _videoService
        /// </summary>
        private readonly IVideoService _videoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="videoService">The videoService<see cref="IVideoService"/></param>
        public HomeController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        /// <summary>
        /// The Index
        /// </summary>
        /// <returns>The <see cref="IActionResult"/></returns>
        public IActionResult Index()
        {
            var videos = _videoService.GetAllVideos();
            return View(videos);
        }

        /// <summary>
        /// The Error
        /// </summary>
        /// <returns>The <see cref="IActionResult"/></returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}
