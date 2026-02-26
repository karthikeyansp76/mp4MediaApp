namespace mp4MediaApp.Services
{
    using mp4MediaApp.Models;

    /// <summary>
    /// Defines the <see cref="IVideoService" />
    /// </summary>
    public interface IVideoService
    {
        /// <summary>
        /// The GetAllVideos
        /// </summary>
        /// <returns>The <see cref="List{VideoFileViewModel}"/></returns>
        List<VideoFileViewModel> GetAllVideos();

        /// <summary>
        /// The UploadVideosAsync
        /// </summary>
        /// <param name="files">The files<see cref="List{IFormFile}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task UploadVideosAsync(List<IFormFile> files);
    }
}
