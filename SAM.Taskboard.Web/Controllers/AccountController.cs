using Microsoft.Owin.Security;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [AllowAnonymous]
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

        #region Login logout
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
                UserServiceResult result = UserService.IsUserEmailConfirmed(model.Email);

                if (result == UserServiceResult.emailNotConfirmed)
                {
                    ModelState.AddModelError("Error", "Your email not confirmed");
                    return View(model);
                }

                ClaimsIdentity claim = UserService.PasswordEmailSignIn(model.Email, model.Password);

                if (claim == null)
                {
                    ModelState.AddModelError("Error", "Invalid email or password");
                }

                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                    }, claim);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
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
                UserServiceResult result = await UserService.Register(model.UserName, model.Email, model.Password);

                if (result == UserServiceResult.error)
                {
                    ModelState.AddModelError("Error", "An error occured, please try again");
                    return View(model);
                }

                else if (result == UserServiceResult.userAlreadyExists)
                {
                    ModelState.AddModelError("Email", "Such user already exist");
                    return View(model);
                }

                else if (result == UserServiceResult.success)
                {
                    string userId = UserService.GetUserIdByEmail(model.Email);
                    string confirmationEmailLink = GetConfirmationLink(userId, model.Email);
                    UserService.SendConfirmationEmail(model.UserName, model.Email, confirmationEmailLink);

                    return RedirectToAction("ConfirmationSent", "Account", new ResendConfirmationEmailViewModel { Email = model.Email });
                }
            }

            return View(model);
        }
        #endregion

        #region EmailConfirmation
        [NonAction]
        public string GetConfirmationLink(string userId, string email)
        {
            string token = UserService.GenerateEmailConfirmationToken(userId);
            return Url.Action("ConfirmEmail", "Account", new { Token = token, UserId = userId, Email = email }, Request.Url.Scheme);
        }

        [HttpGet]
        public ActionResult ConfirmationSent(ResendConfirmationEmailViewModel model)
        {
            if (model.Email == null)
            {
                return RedirectToAction("ResendConfirmation", "Account");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ResendConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResendConfirmation(ResendConfirmationEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = UserService.GetUserIdByEmail(model.Email);

                if (userId == null)
                {
                    ModelState.AddModelError("Error", "There is no user with such email");
                }

                else
                {
                    string confirmationEmailLink = GetConfirmationLink(userId, model.Email);
                    string userName = UserService.GetUserNameByEmail(model.Email);
                    UserServiceResult result = UserService.SendConfirmationEmail(userName, model.Email, confirmationEmailLink);


                    if (result == UserServiceResult.success)
                    {
                        return RedirectToAction("ConfirmationSent", "Account", new ResendConfirmationEmailViewModel { Email = model.Email });
                    }

                    if (result == UserServiceResult.emailNotSent)
                    {
                        ModelState.AddModelError("Error", "Email not sent");
                    }

                    if (result == UserServiceResult.emailAlreadyConfirmed)
                    {
                        ModelState.AddModelError("Error", "Email already confirmed");
                    }
                }
            }

            ModelState.AddModelError("Error", "Something went wrong");
            return View(model);
        }

        [HttpGet]
        public ActionResult ConfirmEmail(string token, string userId, string email)//TODO: maybe remove email from parameters
        {
            UserServiceResult result = UserService.ConfirmEmail(userId, token);

            if (result == UserServiceResult.success)
            {
                return View(new ResendConfirmationEmailViewModel { Email = email });
            }

            if (result == UserServiceResult.emailAlreadyConfirmed)
            {
                return View();// TODO: error handling
            }

            else
            {
                return View(); // TODO: error handling
            }
        }

        #endregion

        #region ResetPassword // TODO: reset password
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
