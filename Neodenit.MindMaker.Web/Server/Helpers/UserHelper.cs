using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Neodenit.MindMaker.Web.Server.Models;

namespace Neodenit.MindMaker.Web.Server.Helpers
{
    public class UserHelper
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserHelper(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
        }

        public async Task<string> GetUserNameAwait( ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            return user.UserName;
        }
    }
}
