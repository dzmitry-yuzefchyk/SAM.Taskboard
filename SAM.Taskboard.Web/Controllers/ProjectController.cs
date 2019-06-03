using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Project;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;
        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        public ActionResult AllProjects(int page = 0)
        {
            string userId = User.Identity.GetUserId();
            ProjectsViewModel model = projectService.GetProjects(userId, page);
            return View(model);
        }

        [HttpGet]
        public ActionResult ProjectsList(int page)
        {
            string userId = User.Identity.GetUserId();
            ProjectsViewModel model = projectService.GetProjects(userId, page);
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult ViewProject(int projectId, int page = 0)
        {
            string userId = User.Identity.GetUserId();
            OperationResult<ProjectViewModel> result = projectService.GetBoards(userId, projectId, page);

            if (result.Message == GenericServiceResult.Error)
            {
                return RedirectToAction("Default", "Error");
            }

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            return View(result.Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = projectService.CreateNewProject(userId, model);

            if (result == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("Error", "Unknown error");
                return PartialView(model);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public JsonResult GetProjectUsers(int projectId)
        {
            string userId = User.Identity.GetUserId();
            OperationResult<Dictionary<string, string>> result = projectService.GetProjectUsers(userId, projectId);

            if (result.Message == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false });
            }

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { accessDenied = true });
            }

            return Json(new { success = true,  users = result.Model }, JsonRequestBehavior.AllowGet);
        }
    }
}