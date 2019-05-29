using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Board;
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

        [HttpGet]
        public ActionResult ViewBoard(int boardId)
        {
            string userId = User.Identity.GetUserId();
            bool isUserHaveAccess = boardService.IsUserHaveAccess(userId, boardId);

            if (!isUserHaveAccess)
            {
                return RedirectToAction("AllProjects", "Project");
            }

            BoardViewModel model = boardService.GetBoard(userId, boardId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddColumn(CreateColumnViewModel model)
        {
            string userId = User.Identity.GetUserId();
            bool isUserHaveAccess = boardService.IsUserHaveAccess(userId, model.BoardId);

            if (!isUserHaveAccess)
            {
                ModelState.AddModelError("Error", "You dont have rights to create a column");
                return PartialView(model);
            }

            GenericServiceResult result = boardService.AddColumn(model);

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
