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

        [HttpGet]
        public ActionResult Settings(int projectId)
        {
            string userId = User.Identity.GetUserId();
            var result = projectService.GetProjectSettings(userId, projectId);

            if (result.Message == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("BadRequest", "Error");
            }

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            return View(result.Model);
        }

        [HttpGet]
        public ActionResult GetSettingsPartial(int projectId)
        {
            string userId = User.Identity.GetUserId();
            var result = projectService.GetProjectSettings(userId, projectId);

            if (result.Message == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("BadRequest", "Error");
            }

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            return PartialView("SettingsProject", result.Model);
        }

        [HttpGet]
        public ActionResult GetProjectUsersPartial(int projectId, int page = 0, string searchFilter = "")
        {
            string userId = User.Identity.GetUserId();
            var result = projectService.GetProjectSettings(userId, projectId, page, searchFilter);

            if (result.Message == GenericServiceResult.Error)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return RedirectToAction("BadRequest", "Error");
            }

            if (result.Message == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            return PartialView("SettingsProjectUsers", result.Model.ProjectUsersViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSettings(ProjectSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("SettingsProject", model);
            }

            string userId = User.Identity.GetUserId();
            var result = projectService.SaveProjectSettings(userId, model);

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

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddUser(int projectId, string userEmailToAdd)
        {
            string userId = User.Identity.GetUserId();
            var result = projectService.AddUserToProject(userId, userEmailToAdd, projectId);

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result == GenericServiceResult.Error)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult DeleteUser(int projectId, string userIdToRemove)
        {
            string userId = User.Identity.GetUserId();
            var result = projectService.RemoveUserFromProject(userId, userIdToRemove, projectId);

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

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult DeleteProject(int projectId)
        {
            string userId = User.Identity.GetUserId();
            var result = projectService.DeleteProject(userId, projectId);

            if (result == GenericServiceResult.AccessDenied)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return RedirectToAction("Forbidden", "Error");
            }

            if (result == GenericServiceResult.Error)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction("AllProjects");
        }
    }
}