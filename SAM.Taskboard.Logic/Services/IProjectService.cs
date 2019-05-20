using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Project;

namespace SAM.Taskboard.Logic.Services
{
    public interface IProjectService
    {
        ProjectsViewModel GetProjects(string userId, int currentPage);
        GenericServiceResult CreateNewProject(string userId, CreateProjectViewModel model);
        ProjectViewModel GetBoards(string userId, int projectId, int currentPage);
        bool IsUserHaveAccess(string userId, int projectId);
    }
}
