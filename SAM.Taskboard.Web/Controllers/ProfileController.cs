using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetUserName()
        {
            return User.Identity.Name;
        }
    }
}