using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Profile;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        public string GetUserName()
        {
            string userId = User.Identity.GetUserId();
            return profileService.GetUserName(userId);
        }

        public string GetUserIcon()
        {
            string userId = User.Identity.GetUserId();
            return profileService.GetUserIcon(userId);
        }

        public string GetNotificationsAmount()
        {
            return User.Identity.GetUserId();
        }

        [HttpGet]
        public ActionResult Settings()
        {
            string userId = User.Identity.GetUserId();
            OperationResult<ProfileSettingsViewModel> result = profileService.GetUserProfile(userId);

            if (result.Message == GenericServiceResult.Error)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(result.Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(ProfileSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = profileService.UpdateUserProfile(model, userId);

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("Error", "Unknown error");
                return View(model);
            }

            var identity = new ClaimsIdentity(User.Identity);

            identity.RemoveClaim(identity.FindFirst("Theme"));
            identity.AddClaim(new Claim("Theme", model.Theme));

            AuthenticationManager.AuthenticationResponseGrant =
                new AuthenticationResponseGrant(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { }
                );

            return View(model);
        }
    }
}