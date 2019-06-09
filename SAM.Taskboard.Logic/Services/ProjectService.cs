using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Project;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Taskboard.Logic.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly int projectsPageSize = 8;
        private readonly int boardsPageSize = 19;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ProjectsViewModel GetProjects(string userId, int currentPage)
        {
            try
            {
                var result = unitOfWork.ProjectUser.Get(
                    projectsPageSize,
                    (currentPage - 1) * projectsPageSize,
                    x => x.Id,
                    u => u.UserId == userId
                    ).ToList();

                List<ProjectInfo> projects = new List<ProjectInfo>();

                foreach (var projectUser in result)
                {
                    int projectId = projectUser.ProjectId;
                    Project project = unitOfWork.Projects.Get(projectId);
                    string projectAbout = project.About;
                    string projectTitle = project.Title;
                    projects.Add(new ProjectInfo { Id = projectId, About = projectAbout, ProjectName = projectTitle });
                }

                int rowsCount = unitOfWork.ProjectUser.Count(u => u.UserId == userId);
                ProjectsViewModel projectsViewModel = new ProjectsViewModel
                {
                    Projects = projects,
                    CurrentPage = currentPage,
                    PageSize = projectsPageSize,
                    RowsCount = rowsCount
                };
                projectsViewModel.GetPages();

                return projectsViewModel;
            }
            catch
            {
                return null;
            }
        }

        public GenericServiceResult CreateNewProject(string userId, CreateProjectViewModel model)
        {
            try
            {
                ProjectSettings projectSettings = new ProjectSettings
                {
                    AccessToChangeProject = (int)model.AccessToChangeProject,
                    AccessToDeleteBoard = (int)model.AccessToDeleteBoard,
                    AccessToCreateBoard = (int)model.AccessToCreateBoard
                };

                Project project = new Project { Title = model.Title, About = model.About, Settings = projectSettings };
                User user = unitOfWork.UserManager.Users.FirstOrDefault(x => x.Id == userId);

                ProjectUser projectUser = new ProjectUser { Project = project, Role = (int)ProjectRoles.Administrator, User = user };
                unitOfWork.ProjectUser.Create(projectUser);

                return GenericServiceResult.Success;
            }

            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public OperationResult<ProjectViewModel> GetBoards(string userId, int projectId, int currentPage)
        {
            try
            {
                bool isUserCanViewProject = IsUserHaveAccess(userId, projectId);
                if (!isUserCanViewProject)
                {
                    return new OperationResult<ProjectViewModel> { Message = GenericServiceResult.AccessDenied, Model = null };
                }

                var result = unitOfWork.Boards.Get
                (
                    boardsPageSize,
                    (currentPage - 1) * boardsPageSize,
                    x => x.Id,
                    u => u.ProjectId == projectId
                ).ToList();

                List<BoardInfo> boards = new List<BoardInfo>();
                foreach (var board in result)
                {
                    int boardId = board.Id;
                    string boardTitle = board.Title;
                    boards.Add(new BoardInfo { Id = boardId, Title = boardTitle });
                }

                string projectTitle = unitOfWork.Projects.Get(projectId).Title;
                int rowsCount = unitOfWork.Boards.Count(u => u.ProjectId == projectId);
                bool canUserCreateBoard = CanUserCreateBoard(userId, projectId);
                bool canUserChangeProject = CanUserChangeProject(userId, projectId);
                ProjectViewModel projectViewModel = new ProjectViewModel
                {
                    ProjectId = projectId,
                    ProjectTitle = projectTitle,
                    CanUserChangeProject = canUserChangeProject,
                    CanUserCreateBoard = canUserCreateBoard,
                    Boards = boards,
                    CurrentPage = currentPage,
                    PageSize = boardsPageSize,
                    RowsCount = rowsCount
                };
                projectViewModel.GetPages();

                return new OperationResult<ProjectViewModel> { Model = projectViewModel, Message = GenericServiceResult.Success };
            }
            catch
            {
                return new OperationResult<ProjectViewModel> { Model = null, Message = GenericServiceResult.Error };
            }
        }

        private bool IsUserHaveAccess(string userId, int projectId)
        {
            try
            {
                return unitOfWork.ProjectUser.GetFirstOrDefaultWhere(u => u.UserId == userId && u.ProjectId == projectId) != null;
            }
            catch
            {
                return false;
            }
        }

        private bool CanUserChangeProject(string userId, int projectId)
        {
            try
            {
                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;
                int roleToChangeProject = unitOfWork.ProjectSettings.GetFirstOrDefaultWhere(x => x.Id == projectId).AccessToChangeProject;

                return roleUserProject <= roleToChangeProject;
            }
            catch
            {
                return false;
            }
        }

        private bool CanUserCreateBoard(string userId, int projectId)
        {
            try
            {
                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;
                int roleToCreateBoard = unitOfWork.ProjectSettings.GetFirstOrDefaultWhere(x => x.Id == projectId).AccessToCreateBoard;

                return roleUserProject <= roleToCreateBoard;
            }
            catch
            {
                return false;
            }
        }

        public OperationResult<Dictionary<string, string>> GetProjectUsers(string userId, int projectId)
        {
            try
            {
                bool isUserCanViewProject = IsUserHaveAccess(userId, projectId);
                if (!isUserCanViewProject)
                {
                    new OperationResult<ProjectViewModel> { Message = GenericServiceResult.AccessDenied, Model = null };
                }

                var projectUsers = unitOfWork.ProjectUser.Get(u => u.ProjectId == projectId).ToList();
                Dictionary<string, string> users = new Dictionary<string, string>();

                foreach (var projectUser in projectUsers)
                {
                    User user = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Id == projectUser.UserId);
                    users.Add(user.Id, user.Email);
                }

                return new OperationResult<Dictionary<string, string>> { Model = users, Message = GenericServiceResult.Success };
            }
            catch
            {
                return new OperationResult<Dictionary<string, string>> { Model = null, Message = GenericServiceResult.Error };
            }
        }
    }
}
