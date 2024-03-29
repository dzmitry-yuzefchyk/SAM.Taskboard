﻿using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Board;
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

        [HttpGet]
        public ActionResult Settings(int boardId)
        {
            string userId = User.Identity.GetUserId();
            var result = boardService.GetBoardSettings(userId, boardId);

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result.Message == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("BadRequest", "Error");
            }

            return View(result.Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSettings(BoardSettingsViewModel model)
        {
            string userId = User.Identity.GetUserId();
            var result = boardService.UpdateBoardSettings(model, userId);

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("BadRequest", "Error");
            }

            return RedirectToAction("Settings", new { boardId = model.BoardId });
        }

        [HttpGet]
        public ActionResult DeleteBoard(int boardId, int projectId)
        {
            string userId = User.Identity.GetUserId();
            var result = boardService.DeleteBoard(userId, boardId);

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("BadRequest", "Error");
            }

            return RedirectToAction("ViewProject", "Project", new { projectId = projectId });
        }

        [HttpGet]
        public ActionResult DeleteColumn(int columnId, int boardId)
        {
            string userId = User.Identity.GetUserId();
            var result = boardService.DeleteColumn(userId, columnId);

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("BadRequest", "Error");
            }

            return RedirectToAction("Settings", "Board", new { boardId = boardId });
        }
    }
}
