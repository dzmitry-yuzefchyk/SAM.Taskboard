using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet]
        public string GetUserName()
        {
            string userId = User.Identity.GetUserId();
            return profileService.GetUserName(userId);
        }
    }
}