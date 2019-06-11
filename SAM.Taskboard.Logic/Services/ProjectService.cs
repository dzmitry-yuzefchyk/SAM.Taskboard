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
                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId && x.ProjectId == projectId).Role;
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

        public OperationResult<ProjectSettingsViewModel> GetProjectSettings(string userId, int projectId, int page = 0, string searchFilter = "")
        {
            try
            {
                bool canUserChangeProject = CanUserChangeProject(userId, projectId);
                if (!canUserChangeProject)
                {
                    return new OperationResult<ProjectSettingsViewModel> { Model = null, Message = GenericServiceResult.AccessDenied };
                }

                Project project = unitOfWork.Projects.Get(projectId);
                ProjectSettings projectSettings = unitOfWork.ProjectSettings.Get(projectId);
                List<ProjectUser> projectUsers = unitOfWork.ProjectUser.Get(p => p.ProjectId == projectId).ToList();
                List<User> users = (from u in unitOfWork.Users.Query()
                                    join r in projectUsers on u.Id equals r.UserId
                                    where u.UserName.Contains(searchFilter)
                                    select u
                                    ).Skip(page * 15).Take(15).ToList();

                int rowsCount = projectUsers.Count;

                List<UserInfo> userInfos = new List<UserInfo>();

                foreach (var user in users)
                {
                    UserProfile profile = unitOfWork.ClientManager.GetProfile(user.Id);
                    int userRole = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.UserId == user.Id && p.ProjectId == projectId).Role;
                    bool isUserAdministrator = (ProjectRoles)userRole == ProjectRoles.Administrator;
                    bool canYouDeleteUser = isUserAdministrator || userId == user.Id;
                    UserInfo userInfo = new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Icon = profile.Icon,
                        Name = profile.Name,
                        CanYouDeleteUser = !canYouDeleteUser
                    };
                    userInfos.Add(userInfo);
                }

                ProjectUsersViewModel projectUsersModel = new ProjectUsersViewModel
                {
                    ProjectUsers = userInfos,
                    CurrentPage = page,
                    PageSize = 15,
                    RowsCount = rowsCount
                };

                projectUsersModel.GetPages();

                bool canUserDeleteProject = CanUserDeleteProject(userId, projectId);

                ProjectSettingsViewModel model = new ProjectSettingsViewModel
                {
                    Id = projectId,
                    Title = project.Title,
                    About = project.About,
                    AccessToChangeProject = (ProjectSettingsRole)projectSettings.AccessToChangeProject,
                    AccessToCreateBoard = (BoardSettingsRole)projectSettings.AccessToCreateBoard,
                    AccessToDeleteBoard = (BoardSettingsRole)projectSettings.AccessToDeleteBoard,
                    ProjectUsersViewModel = projectUsersModel,
                    CanUserDeleteProject = canUserDeleteProject
                };

                return new OperationResult<ProjectSettingsViewModel> { Model = model, Message = GenericServiceResult.Success };
            }
            catch
            {
                return new OperationResult<ProjectSettingsViewModel> { Model = null, Message = GenericServiceResult.Error };
            }
        }

        public GenericServiceResult SaveProjectSettings(string userId, ProjectSettingsViewModel model)
        {
            try
            {
                int projectId = model.Id;
                bool canUserChangeProject = CanUserChangeProject(userId, projectId);
                if (!canUserChangeProject)
                {
                    return GenericServiceResult.AccessDenied;
                }

                Project project = unitOfWork.Projects.Get(projectId);
                ProjectSettings projectSettings = unitOfWork.ProjectSettings.Get(projectId);

                project.Title = model.Title;
                project.About = model.About;
                projectSettings.AccessToChangeProject = (int)model.AccessToChangeProject;
                projectSettings.AccessToCreateBoard = (int)model.AccessToCreateBoard;
                projectSettings.AccessToDeleteBoard = (int)model.AccessToDeleteBoard;

                unitOfWork.ProjectSettings.Update(projectSettings);
                unitOfWork.Projects.Update(project);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult AddUserToProject(string userId, string userEmailToAdd, int projectId)
        {
            try
            {
                bool canUserChangeProject = CanUserChangeProject(userId, projectId);
                if (!canUserChangeProject)
                {
                    return GenericServiceResult.AccessDenied;
                }

                User existingUser = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Email == userEmailToAdd);
                if (existingUser == null)
                {
                    return GenericServiceResult.Error;
                }

                string userIdToAdd = existingUser.Id;

                ProjectUser existingProjectUser = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.UserId == userIdToAdd && p.ProjectId == projectId);
                if (existingProjectUser != null)
                {
                    return GenericServiceResult.Error;
                }

                ProjectUser projectUser = new ProjectUser
                {
                    ProjectId = projectId,
                    UserId = userIdToAdd,
                    Role = (int)ProjectRoles.Member
                };

                unitOfWork.ProjectUser.Create(projectUser);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult RemoveUserFromProject(string userId, string userIdToRemove, int projectId)
        {
            try
            {
                bool canUserChangeProject = CanUserChangeProject(userId, projectId);
                if (!canUserChangeProject)
                {
                    return GenericServiceResult.AccessDenied;
                }

                ProjectUser projectUser = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == projectId && p.UserId == userIdToRemove);
                unitOfWork.ProjectUser.Delete(projectUser.Id);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult DeleteProject(string userId, int projectId)
        {
            try
            {
                bool canUserDeleteProject = CanUserDeleteProject(userId, projectId);
                if (!canUserDeleteProject)
                {
                    return GenericServiceResult.AccessDenied;
                }

                List<Board> boards = unitOfWork.Boards.Get(b => b.ProjectId == projectId).ToList();
                var result = DeleteBoards(boards);
                if (result == GenericServiceResult.Error)
                {
                    return result;
                }

                unitOfWork.ProjectUser.Delete(p => p.ProjectId == projectId);
                unitOfWork.ProjectSettings.Delete(projectId);
                unitOfWork.Projects.Delete(projectId);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private GenericServiceResult DeleteBoards(List<Board> boards)
        {
            try
            {
                foreach (var board in boards)
                {
                    List<Column> columns = unitOfWork.Columns.Get(c => c.BoardId == board.Id).ToList();
                    DeleteColumns(columns);
                    unitOfWork.BoardSettings.Delete(board.Id);
                    unitOfWork.Boards.Delete(board.Id);
                }

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private GenericServiceResult DeleteColumns(List<Column> columns)
        {
            try
            {
                foreach (var column in columns)
                {
                    List<Task> tasks = unitOfWork.Tasks.Get(t => t.ColumnId == column.Id).ToList();
                    DeleteTasks(tasks);
                    unitOfWork.Columns.Delete(column.Id);
                }

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private GenericServiceResult DeleteTasks(List<Task> tasks)
        {
            try
            {
                foreach (var task in tasks)
                {
                    unitOfWork.Attachments.Delete(a => a.TaskId == task.Id);
                    unitOfWork.Tasks.Delete(task.Id);
                }

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private bool CanUserDeleteProject(string userId, int projectId)
        {
            try
            {
                ProjectUser projectUser = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == projectId && p.UserId == userId);
                if (projectUser == null)
                {
                    return false;
                }

                if ((ProjectRoles)projectUser.Role != ProjectRoles.Administrator)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
