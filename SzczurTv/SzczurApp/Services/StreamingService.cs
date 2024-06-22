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

        public StreamingService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _streamingUrl = configuration["StreamingUrl"] ?? throw new Exception("Streaming server URL not configured");
            _watchStreamUrl = $"{configuration["DomainUrl"]}/watch-stream"
        }

        public async Task<string> GenerateStreamKeyAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.StreamKey = Guid.NewGuid().ToString("N");
            await _userManager.UpdateAsync(user);

            return user.StreamKey;
        }

        public async Task<string> GetStreamUrlAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.StreamKey))
                throw new Exception("Stream key not found");

            return $"{_streamingUrl}/{user.StreamKey}.m3u8";
        }

        public async Task<string> GetWatchStreamUrlAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return $"{_watchStreamUrl}/{user.Id}";
        }
    }
}
