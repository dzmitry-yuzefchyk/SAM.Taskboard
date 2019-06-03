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

        [ValidateAntiForgeryToken]
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
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MoveTaskToColumn(int taskId, int columnId, int boardId)
        {
            string userId = User.Identity.GetUserId();
            GenericServiceResult result = taskService.MoveTask(userId, boardId, taskId, columnId);

            if (result == GenericServiceResult.AccessDenied)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}