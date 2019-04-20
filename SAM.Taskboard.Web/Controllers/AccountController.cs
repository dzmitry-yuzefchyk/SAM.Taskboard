using Microsoft.Owin.Security;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private IUserService UserService { get; set; }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        #endregion

        public AccountController(IUserService userService)
        {
            this.UserService = userService;
        }

        #region Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {


                return View();
            }

            return View(model);
        }
        #endregion

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Result result = await UserService.Register(model.UserName, model.Email, model.Password);

                if (result == Result.error)
                {
                    ModelState.AddModelError("", "An error occured, please try again");
                    return View(model);
                }

                else if (result == Result.userAlreadyExists)
                {
                    ModelState.AddModelError("Email", "An error occured, please try again");
                    return View(model);
                }

                else if (result == Result.success)
                {
                    string userId = UserService.GetUserId(model.Email);
                    string confirmationEmailLink = GetConfirmationLink(userId, model.Email);
                    result = UserService.SendConfirmationEmail(model.UserName, model.Email, confirmationEmailLink);

                    if (result == Result.success)
                    {
                        return RedirectToAction("Confirm", "Account", new { Email = model.Email });
                    }
                    else
                    {
                        return RedirectToAction("ResendConfirmation", "Account", new { Email = model.Email });
                    }
                }
            }

            return View(model);
        }
        #endregion

        #region EmailConfirmation
        [NonAction]
        public string GetConfirmationLink(string userId, string email)
        {
            return Url.Action("ConfirmEmail", "Account", new { Token = userId, Email = email }, Request.Url.Scheme); //TODO: ask about it
        }

        #endregion

        #region ResetPassword
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(bool f)
        {
            return View();
        }
        #endregion
    }
}
