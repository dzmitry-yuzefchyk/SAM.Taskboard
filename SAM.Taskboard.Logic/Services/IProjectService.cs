using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Project;
using System.Collections.Generic;

namespace SAM.Taskboard.Logic.Services
{
    public interface IProjectService
    {
        ProjectsViewModel GetProjects(string userId, int currentPage);
        GenericServiceResult CreateNewProject(string userId, CreateProjectViewModel model);
        OperationResult<ProjectViewModel> GetBoards(string userId, int projectId, int currentPage);
        OperationResult<Dictionary<string, string>> GetProjectUsers(string userId, int projectId);
    }
}
