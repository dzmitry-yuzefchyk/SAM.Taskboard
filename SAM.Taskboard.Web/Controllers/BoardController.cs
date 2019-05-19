using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Board;
using System.Web;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private readonly IBoardService boardService;
        public BoardController(IBoardService boardService)
        {
            this.boardService = boardService;
        }

        [HttpPost]
        public ActionResult CreateBoard(CreateBoardViewModel model, int projectId)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = boardService.CreateBoard(model, userId, projectId);

            if (result == GenericServiceResult.Error)
            {
                ModelState.AddModelError("Error", "Unknown error");
                return PartialView(model);
            }

            else
            {
                return Json(new { success = true });
            }
        }
    }
}
