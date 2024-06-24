using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SzczurApp.Data;

namespace SzczurApp.Services
{
    public class StreamingService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _streamingUrl;
        private readonly string _watchStreamUrl;
        public readonly string RTMP_URL;

        public StreamingService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _streamingUrl =
                configuration["StreamingUrl"]
                ?? throw new Exception("Streaming server URL not configured");
            _watchStreamUrl = $"{configuration["DomainUrl"]}/user";
            var domainName = configuration["DomainUrl"].Split("://")[1];

            RTMP_URL = $"rtmp://{domainName}/live";
        }

        public async Task<string> GenerateStreamKeyAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.StreamKey = Guid.NewGuid().ToString("N");
            await _userManager.UpdateAsync(user);

            return user.StreamKey;
        }

        public async Task<string> GetStreamKeyAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (string.IsNullOrEmpty(user.StreamKey))
            {
                return "No stream generated yet";
            }
            return user.StreamKey;
        }

        public async Task<string> GetStreamUrlAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || string.IsNullOrEmpty(user.StreamKey))
                throw new Exception("Stream key not found");

            return $"{_streamingUrl}/{user.StreamKey}.m3u8";
        }

        public async Task<string?> GetStreamUrlContainerAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || string.IsNullOrEmpty(user.StreamKey))
                return null;

            return $"http://nginx_rtmp:8080/hls/{user.StreamKey}.m3u8";
        }

        public async Task<string> GetWatchStreamUrlAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return $"{_watchStreamUrl}/{user.UserName}";
        }

        public async Task<List<string>> GetAllStreamWatchUrlsAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var watchUrls = new List<string>();

            foreach (var user in users)
            {
                var watchStreamUrl = await GetWatchStreamUrlAsync(user.UserName);
                watchUrls.Add(watchStreamUrl);
            }

            return watchUrls;
        }

        public async Task<bool> IsStreamActiveAsync(string userName)
        {
            var streamUrl = await GetStreamUrlContainerAsync(userName);

            if (streamUrl == null)
            {
                Console.WriteLine($"Stream URL is null for user: {userName}");
                return false;
            }

            using (var client = new HttpClient())
            {
                Console.WriteLine($"Checking stream URL: {streamUrl}");

                var response = await client.GetAsync(streamUrl);

                if (response.IsSuccessStatusCode)
                {
                    var contentType = response.Content.Headers.ContentType?.MediaType;
                    if (contentType == "application/vnd.apple.mpegurl" || contentType == "application/x-mpegURL")
                    {
                        Console.WriteLine($"Stream is active and returns a valid m3u8 file: {streamUrl}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Stream is active but does not return a valid m3u8 file: {streamUrl}. Content-Type: {contentType}");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"Stream is not active: {streamUrl}. Status code: {response.StatusCode}");
                    return false;
                }
            }
        }
    }
}
