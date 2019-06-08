using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Board;
using System.IO;
using System.Net;
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
        public ActionResult ViewBoard(int boardId, string orderBy = "priority", string direction = "ASC", string search = "", bool assignedToMe = false)
        {
            string userId = User.Identity.GetUserId();
            OperationResult<BoardViewModel> result = boardService.GetBoard(userId, boardId, orderBy, direction, search, assignedToMe);

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            return View(result.Model);
        }

        [HttpGet]
        public PartialViewResult PartialViewBoard(int boardId, string orderBy = "priority", string direction = "ASC", string search = "", bool assignedToMe = false)
        {
            string userId = User.Identity.GetUserId();
            OperationResult<BoardViewModel> result = boardService.GetBoard(userId, boardId, orderBy, direction, search, assignedToMe);

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }

            return PartialView("Board", result.Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBoard(CreateBoardViewModel model, int projectId)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = boardService.CreateBoard(model, userId, projectId);

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("Error", "Unknown error");
                return PartialView(model);
            }

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateColumn(CreateColumnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = boardService.AddColumn(model, userId);

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("Error", "Unknown error");
                return PartialView(model);
            }

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
