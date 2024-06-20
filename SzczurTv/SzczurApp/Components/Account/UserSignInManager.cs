using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SzczurApp.Data;

namespace SzczurApp.Components.Account
{
    public class UserSignInManager : SignInManager<ApplicationUser>
    {
        public UserSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(u => u.UserName == userName || u.Email == userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }
    }
}
