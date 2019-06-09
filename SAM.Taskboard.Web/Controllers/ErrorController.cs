using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Default(string aspxerrorpath)
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotFound(string aspxerrorpath)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Forbidden(string aspxerrorpath)
        {
            return View();
        }

        [HttpGet]
        public ActionResult BadRequest(string aspxerrorpath)
        {
            return View();
        }
    }
}