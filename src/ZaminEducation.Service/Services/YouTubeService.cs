using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YoutubeExplode;
using YoutubeExplode.Common;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;


namespace ZaminEducation.Service.Services
{
    public class YouTubeService : IYouTubeService
    {
        private readonly IRepository<CourseVideo> youtubeRepository;
        private readonly IRepository<Course> courseRepository;

        public YouTubeService(
            IRepository<CourseVideo> youtubeRepository,
            IRepository<Course> courseRepository)
        {
            this.youtubeRepository = youtubeRepository;
            this.courseRepository = courseRepository;
        }

        public async ValueTask<CourseVideo> CreateAsync(string link, long courseId)
        {
            var course = await courseRepository.GetAsync(course => course.Id == courseId);

            if (course is null)
                throw new ZaminEducationException(404, "Course not found!");

            var video = await new YoutubeClient().Videos.GetAsync(YouTubeVideoIdExtractor(link));

            var youtubeVideo = new CourseVideo
            {
                CourseId = courseId,
                Thumbnail = video.Thumbnails.OrderByDescending(p => p.Resolution.Height).FirstOrDefault()?.Url,
                Title = video.Title,
                Url = video.Url,
                Length = video.Duration!.Value.Minutes,
                Description = video.Description
            };

            youtubeVideo = await youtubeRepository.AddAsync(youtubeVideo);
            youtubeVideo.Create();
            await youtubeRepository.SaveChangesAsync();

            return youtubeVideo;
        }

        public async ValueTask<IEnumerable<CourseVideo>> CreateRangeAsync(string youtubePlaylist, long courseId)
        {
            IEnumerable<string> links = await GetLinksAsync(youtubePlaylist);

            var videos = new List<CourseVideo>();
            var yt = new YoutubeClient();

            foreach (var link in links)
            {
                var video = await yt.Videos.GetAsync(link);

                var youtubeVideo = new CourseVideo
                {
                    CourseId = courseId,
                    Thumbnail = video.Thumbnails.OrderByDescending(p => p.Resolution.Height).FirstOrDefault()?.Url,
                    Title = video.Title,
                    Url = video.Url,
                    Length = video.Duration!.Value.Minutes,
                    Description = video.Description
                };

                youtubeVideo.Create();
                videos.Add(await youtubeRepository.AddAsync(youtubeVideo));
            }

            await youtubeRepository.SaveChangesAsync();

            return videos;
        }

        public async ValueTask<bool> DeleteAsync(long youtubeId)
        {
            var existVideo = await youtubeRepository.GetAsync(p => p.Id == youtubeId);

            if (existVideo is null)
                throw new ZaminEducationException(404, "Video not found!");

            youtubeRepository.Delete(existVideo);
            await youtubeRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<CourseVideo> UpdateAsync(long courseId, string link)
        {
            var existVideo = await youtubeRepository.GetAsync(p => p.Id == courseId);

            if (existVideo is null)
                throw new ZaminEducationException(404, "Video not found!");

            var video = await new YoutubeClient().Videos.GetAsync(YouTubeVideoIdExtractor(link));

            var youtubeVideo = new CourseVideo
            {
                Thumbnail = video.Thumbnails.OrderByDescending(p => p.Resolution.Height).FirstOrDefault()?.Url,
                Title = video.Title,
                Url = video.Url,
                Length = video.Duration!.Value.Minutes,
                Description = video.Description,
                CourseId = existVideo.CourseId,
                CreatedAt = existVideo.CreatedAt,
                UpdatedAt = existVideo.UpdatedAt,
                Id = existVideo.Id
            };

            youtubeVideo = youtubeRepository.Update(youtubeVideo);
            youtubeVideo.Update();
            await youtubeRepository.SaveChangesAsync();

            return youtubeVideo;
        }

        public async ValueTask<CourseVideo> GetAsync(Expression<Func<CourseVideo, bool>> expression)
        {
            var video = await youtubeRepository.GetAsync(expression);

            if (video is null)
                throw new ZaminEducationException(404, "Video not found!");

            return video;
        }

        public async ValueTask<IEnumerable<CourseVideo>> GetAllAsync(
            PaginationParams @params, Expression<Func<CourseVideo, bool>> expression = null) =>
                await youtubeRepository.GetAll(expression)?.ToPagedList(@params).ToListAsync();

        public async ValueTask<IEnumerable<CourseVideo>> GetAllAsync(
            PaginationParams @params, string search) =>
                await youtubeRepository.GetAll(cv => cv.Title == search 
                    ||cv.Description == search)? 
                    .ToPagedList(@params).ToListAsync();

        public async ValueTask<IEnumerable<string>> GetLinksAsync(string playlistLink)
        {
            if (!playlistLink.Contains("list") || !IsYouTubeLink(playlistLink))
                throw new ZaminEducationException(404, "Invalid Playlist Url");

            playlistLink = playlistLink.Split("list=")[1].Split("&")[0];

            var yt = new YoutubeClient();

            var videos = await yt.Playlists.GetVideosAsync(playlistLink);

            return videos.Select(p => p.Url);
        }

        public async ValueTask SetModuleIdAsync(long[] Ids, long courseId, long moduleId)
        {
            foreach (long id in Ids)
            {
                var video = await youtubeRepository.GetAsync(yu => yu.CourseId == id && yu.Id == id);

                if (video is not null)
                    video.Id = moduleId;
            }

            await youtubeRepository.SaveChangesAsync();
        }

        private string YouTubeVideoIdExtractor(string link)
        {
            // Samples
            // https://www.youtube.com/watch?v=9Pv0Q8zFGP0&ab_channel=NajotTa%27lim
            // https://www.youtube.com/watch?v=5IanQIwhA4E

            if (!IsYouTubeLink(link))
                throw new ZaminEducationException(400, "Invalid Youtube link");

            return link.Split("&")[0].Split('=')[1];
        }

        private bool IsYouTubeLink(string link)
        {
            try
            {
                Uri uri = new Uri(link);

                return uri.Host == "www.youtube.com" || uri.Host == "youtube.com" ||
                        uri.Host == "www.youtu.be" || uri.Host == "youtu.be";
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
