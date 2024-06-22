using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
            var user = await _userManager.FindByIdAsync(userName);
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

        public async Task<string> GetWatchStreamUrlAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return $"{_watchStreamUrl}/{user.UserName}";
        }
    }
}
