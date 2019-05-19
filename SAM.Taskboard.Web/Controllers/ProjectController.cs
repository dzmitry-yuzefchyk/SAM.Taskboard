using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Project;
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
            ProjectViewModel model = projectService.GetBoards(userId, projectId, page);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = projectService.CreateNewProject(userId, model);

            if (result == GenericServiceResult.Error)
            {
                ModelState.AddModelError("Error", "Unknown error");
                return PartialView(model);
            }

            else
            {
                return Json(new { success = true } );
            }
        }
    }
}