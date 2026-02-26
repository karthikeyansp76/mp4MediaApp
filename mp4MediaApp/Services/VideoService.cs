namespace mp4MediaApp.Services
{
    using mp4MediaApp.Models;

    /// <summary>
    /// Defines the <see cref="VideoService" />
    /// </summary>
    public class VideoService : IVideoService
    {
        /// <summary>
        /// Defines the _env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Defines the MaxFileSize
        /// </summary>
        private const long MaxFileSize = 200_000_000;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoService"/> class.
        /// </summary>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/></param>
        public VideoService(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// The GetAllVideos
        /// </summary>
        /// <returns>The <see cref="List{VideoFileViewModel}"/></returns>
        public List<VideoFileViewModel> GetAllVideos()
        {
            var mediaPath = Path.Combine(_env.WebRootPath, "media");

            if (!Directory.Exists(mediaPath))
                Directory.CreateDirectory(mediaPath);

            return Directory.GetFiles(mediaPath)
                .Select(f => new VideoFileViewModel
                {
                    FileName = Path.GetFileName(f),
                    FileSize = new FileInfo(f).Length
                })
                .OrderByDescending(v => v.FileName)
                .ToList();
        }

        /// <summary>
        /// The UploadVideosAsync
        /// </summary>
        /// <param name="files">The files<see cref="List{IFormFile}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task UploadVideosAsync(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("No files selected.");

            var mediaPath = Path.Combine(_env.WebRootPath, "media");

            if (!Directory.Exists(mediaPath))
                Directory.CreateDirectory(mediaPath);

            foreach (var file in files)
            {
                ValidateFile(file);

                var safeFileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(mediaPath, safeFileName);

                using var stream = new FileStream(
                                     filePath,
                                     FileMode.Create,
                                     FileAccess.Write,
                                     FileShare.None,
                                     81920,
                                     useAsync: true);

                await file.CopyToAsync(stream);
            }
        }

        /// <summary>
        /// The ValidateFile
        /// </summary>
        /// <param name="file">The file<see cref="IFormFile"/></param>
        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (extension != ".mp4")
                throw new ArgumentException($"File '{file.FileName}' is not an MP4 file.");

            if (file.Length > MaxFileSize)
                throw new InvalidOperationException(
                    $"File '{file.FileName}' exceeds the 200MB limit.");
        }
    }
}
