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
        OperationResult<ProjectSettingsViewModel> GetProjectSettings(string userId, int projectId, int page = 0, string searchFilter = "");
        GenericServiceResult SaveProjectSettings(string userId, ProjectSettingsViewModel model);
        GenericServiceResult AddUserToProject(string userId, string userEmailToAdd, int projectId);
        GenericServiceResult RemoveUserFromProject(string userId, string userIdToRemove, int projectId);
        GenericServiceResult DeleteProject(string userId, int projectId);
    }
}
