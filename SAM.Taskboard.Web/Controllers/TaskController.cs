using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Task;
using System.Net;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        public ActionResult CreateTask(CreateTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = taskService.CreateTask(model, userId);

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("Error", "Unknown error");
                return PartialView(model);
            }

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("Default", "Error");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTask(TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            string userId = User.Identity.GetUserId();
            var result = taskService.UpdateTask(model, userId);

            if (result == GenericServiceResult.AccessDenied)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (result == GenericServiceResult.Error)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult DeleteTask(int taskId, int boardId)
        {
            string userId = User.Identity.GetUserId();
            var result = taskService.DeleteTask(userId, taskId, boardId);

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("Default", "Error");
            }

            return RedirectToAction("ViewBoard", "Board", new { BoardId = boardId });
        }

        [HttpGet]
        public ActionResult ViewTask(int taskId)
        {
            string userId = User.Identity.GetUserId();
            var result = taskService.ViewTask(userId, taskId);

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result.Message == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("Default", "Error");
            }

            return View(result.Model);
        }

        [HttpPost]
        public ActionResult MoveTaskToColumn(int taskId, int columnId, int boardId)
        {
            string userId = User.Identity.GetUserId();
            GenericServiceResult result = taskService.MoveTask(userId, boardId, taskId, columnId);

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("Default", "Error");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        [HttpGet]
        public ActionResult DownloadAttachment(int attachmentId, int projectId)
        {
            string userId = User.Identity.GetUserId();
            OperationResult<AttachmentInfo> result = taskService.GetAttachment(userId, attachmentId, projectId);

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result.Message == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("Default", "Error");
            }

            AttachmentInfo attachment = result.Model;
            return File(attachment.Document, attachment.FileType, attachment.FileName);
        }
    }
}