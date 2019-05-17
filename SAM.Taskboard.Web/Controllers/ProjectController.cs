using Microsoft.AspNet.Identity;
using SAM.Taskboard.Logic.Services;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
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
            ProjectsViewModel projectsViewModel = projectService.GetProjects(userId, page);
            return View(projectsViewModel);
        }
        [HttpGet]
        public ActionResult View(int projectId, int page)
        {
            string userId = User.Identity.GetUserId();
            projectService.GetBoards(userId, projectId, page);
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = User.Identity.GetUserId();
            GenericServiceResult result = projectService.CreateNewProject(userId, model);

            if (result == GenericServiceResult.Error)
            {
                ModelState.AddModelError("Error", "Unknown error");
                return View(model);
            }

            else
            {
                return RedirectToAction("AllProjects");
            }
        }
    }
}