using System.Web;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        [HttpGet]
        public ActionResult CreateBoard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBoard(int i)
        {
            return View();
        }
    }
}
