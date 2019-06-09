using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Board;
using SAM.Taskboard.Model.Project;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Taskboard.Logic.Services
{
    public class BoardService : IBoardService
    {
        private readonly IUnitOfWork unitOfWork;

        public BoardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public OperationResult<BoardViewModel> GetBoard(string userId, int boardId, string orderBy, string direction, string search, bool assignedToMe)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;
                bool isUserHaveAccess = IsUserHaveAccess(userId, projectId);

                if (!isUserHaveAccess)
                {
                    return new OperationResult<BoardViewModel> { Model = null, Message = GenericServiceResult.AccessDenied };
                }

                string projectTitle = unitOfWork.Projects.Get(projectId).Title;

                bool canUserChangeBoard = CanUserChangeBoard(userId, boardId);
                bool canUserCreateTask = CanUserCreateTask(userId, boardId);

                List<ColumnInfo> columnInfos = GetColumns(userId, boardId, orderBy, direction, search, assignedToMe);
                List<Board> boards = unitOfWork.Boards.Get(b => b.ProjectId == projectId && b.Id != boardId).ToList();
                List<BoardInfo> boardInfos = new List<BoardInfo>();

                foreach (var b in boards)
                {
                    boardInfos.Add(new BoardInfo { Id = b.Id, Title = b.Title });
                }

                BoardViewModel model = new BoardViewModel()
                {
                    CanUserChangeBoard = canUserChangeBoard,
                    CanUserCreateTask = canUserCreateTask,
                    Columns = columnInfos,
                    Title = board.Title,
                    BoardId = board.Id,
                    ProjectId = projectId,
                    ProjectTitle = projectTitle,
                    AssignedFilter = assignedToMe,
                    OrderFilter = orderBy,
                    OrderDirection = direction,
                    SearchFilter = search,
                    Boards = boardInfos
                };

                return new OperationResult<BoardViewModel> { Model = model, Message = GenericServiceResult.Success };
            }
            catch
            {
                return null;
            }
        }

        private List<ColumnInfo> GetColumns(string userId, int boardId, string orderBy, string direction, string search, bool assignedToMe)
        {
            try
            {
                List<Column> columns = unitOfWork.Columns.Get(c => c.BoardId == boardId).OrderBy(c => c.Position).ToList();
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();

                foreach (var column in columns)
                {
                    List<TaskInfo> taskInfos = GetTasks(userId, column.Id, boardId, orderBy, direction, search, assignedToMe);

                    columnInfos.Add(new ColumnInfo
                    {
                        Id = column.Id,
                        Title = column.Title,
                        Tasks = taskInfos
                    });
                }

                return columnInfos;
            }
            catch
            {
                return new List<ColumnInfo>();
            }
        }

        private List<ColumnInfo> GetColumns(int boardId)
        {
            try
            {
                List<Column> columns = unitOfWork.Columns.Get(c => c.BoardId == boardId).OrderBy(c => c.Position).ToList();
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();

                foreach (var column in columns)
                {
                    columnInfos.Add(new ColumnInfo
                    {
                        Id = column.Id,
                        Title = column.Title,
                        Position = column.Position
                    });
                }

                return columnInfos;
            }
            catch
            {
                return new List<ColumnInfo>();
            }
        }

        private List<TaskInfo> GetTasks(string userId, int columnId, int boardId, string orderBy, string direction, string search, bool assignedToMe)
        {
            try
            {
                Func<Task, object> orderByFilter = t => t.Priority;
                Func<Task, bool> whereFilter = t => t.ColumnId == columnId;

                if (orderBy.ToLower() == "severity")
                {
                    orderByFilter = t => t.Severity;
                }

                if (assignedToMe)
                {
                    whereFilter = t => t.ColumnId == columnId && t.AssigneeId == userId;
                }

                List<Task> tasks = unitOfWork.Tasks.Get(whereFilter, orderByFilter, direction).ToList();

                if (search != "")
                {
                    tasks = (from t in tasks
                             where t.Title.Contains(search)
                             select t).ToList();
                }

                List<TaskInfo> taskInfos = new List<TaskInfo>();

                foreach (var task in tasks)
                {
                    User assignee = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Id == task.AssigneeId);
                    UserProfile assigneeProfile = unitOfWork.UserProfiles.GetFirstOrDefaultWhere(p => p.Id == task.AssigneeId);
                    User creator = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Id == task.CreatorId);
                    UserProfile creatorProfile = unitOfWork.UserProfiles.GetFirstOrDefaultWhere(p => p.Id == task.CreatorId);

                    bool canUserChangeTask = CanUserChangeTask(userId, boardId, task.Id);

                    taskInfos.Add(new TaskInfo
                    {
                        Id = task.Id,
                        Title = task.Title,
                        AssigneeIcon = assignee == null ? null : assigneeProfile.Icon,
                        AssigneeEmail = assignee == null ? "Unassigned" : assignee.Email,
                        CreatorIcon = creatorProfile.Icon,
                        CreatorEmail = creator.Email,
                        CanUserChangeTask = canUserChangeTask,
                        Priority = (Priority)task.Priority,
                        Severity = (Severity)task.Severity,
                        Type = (TaskType)task.Type,
                        CreationTime = task.StartTime
                    });
                }

                return taskInfos;
            }
            catch
            {
                return new List<TaskInfo>();
            }
        }

        public GenericServiceResult CreateBoard(CreateBoardViewModel model, string userId, int projectId)
        {
            try
            {
                bool isUserCanCreateBoard = CanUserCreateBoard(userId, projectId);

                if (!isUserCanCreateBoard)
                {
                    return GenericServiceResult.AccessDenied;
                }

                ProjectUser projectUser = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == projectId && p.UserId == userId);

                BoardSettings boardSettings = new BoardSettings
                {
                    AccessToChangeTask = (int)model.AccessToChangeTask,
                    AccessToCreateTask = (int)model.AccessToCreateTask,
                    AccessToDeleteTask = (int)model.AccessToDeleteTask,
                    AccessToChangeBoard = (int)model.AccessToChangeBoard
                };

                Board board = new Board
                {
                    Title = model.Title,
                    Settings = boardSettings,
                    ProjectId = projectId,
                    CreatorId = userId
                };

                unitOfWork.Boards.Create(board);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult AddColumn(CreateColumnViewModel model, string userId)
        {
            try
            {
                bool isUserCanChangeBoard = CanUserChangeBoard(userId, model.BoardId);

                if (!isUserCanChangeBoard)
                {
                    return GenericServiceResult.AccessDenied;
                }

                Column lastColumn = unitOfWork.Columns.Get(c => c.BoardId == model.BoardId).OrderBy(c => c.Position).LastOrDefault();
                int newColumnPosition = lastColumn == null ? 0 : lastColumn.Position + 1;
                Column column = new Column { Title = model.Title, BoardId = model.BoardId, Position = newColumnPosition };
                unitOfWork.Columns.Create(column);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public OperationResult<BoardSettingsViewModel> GetBoardSettings(string userId, int boardId)
        {
            try
            {
                bool isUserCanChangeBoard = CanUserChangeBoard(userId, boardId);

                if (!isUserCanChangeBoard)
                {
                    return new OperationResult<BoardSettingsViewModel> { Model = null, Message = GenericServiceResult.AccessDenied };
                }

                Board board = unitOfWork.Boards.Get(boardId);
                BoardSettings boardSettings = unitOfWork.BoardSettings.Get(boardId);
                Project project = unitOfWork.Projects.Get(board.ProjectId);

                List<ColumnInfo> columns = GetColumns(boardId);

                List<Board> boards = unitOfWork.Boards.Get(b => b.ProjectId == project.Id && b.Id != board.Id).ToList();
                List<BoardInfo> boardInfos = new List<BoardInfo>();

                foreach (var b in boards)
                {
                    BoardInfo boardInfo = new BoardInfo { Id = b.Id, Title = b.Title };
                    boardInfos.Add(boardInfo);
                }

                BoardSettingsViewModel model = new BoardSettingsViewModel
                {
                    Title = board.Title,
                    BoardId = board.Id,
                    AccessToChangeBoard = (TaskSettingsRole)boardSettings.AccessToChangeBoard,
                    AccessToChangeTask = (TaskSettingsRole)boardSettings.AccessToChangeTask,
                    AccessToCreateTask = (TaskSettingsRole)boardSettings.AccessToCreateTask,
                    AccessToDeleteTask = (TaskSettingsRole)boardSettings.AccessToDeleteTask,
                    ProjectId = project.Id,
                    ProjectTitle = project.Title,
                    Columns = columns,
                    Boards = boardInfos
                };

                return new OperationResult<BoardSettingsViewModel> { Model = model, Message = GenericServiceResult.Success };
            }
            catch
            {
                return new OperationResult<BoardSettingsViewModel> { Model = null, Message = GenericServiceResult.Error };
            }
        }

        public GenericServiceResult UpdateBoardSettings(BoardSettingsViewModel model, string userId)
        {
            try
            {
                bool isUserCanChangeBoard = CanUserChangeBoard(userId, model.BoardId);

                if (!isUserCanChangeBoard)
                {
                    return GenericServiceResult.AccessDenied;
                }

                Board board = unitOfWork.Boards.Get(model.BoardId);
                BoardSettings boardSettings = unitOfWork.BoardSettings.Get(model.BoardId);

                board.Title = model.Title;
                boardSettings.AccessToChangeBoard = (int)model.AccessToChangeBoard;
                boardSettings.AccessToChangeTask = (int)model.AccessToChangeTask;
                boardSettings.AccessToCreateTask = (int)model.AccessToCreateTask;
                boardSettings.AccessToDeleteTask = (int)model.AccessToDeleteTask;

                unitOfWork.BoardSettings.Update(boardSettings);
                unitOfWork.Boards.Update(board);

                foreach (var column in model.Columns)
                {
                    Column databaseColumn = unitOfWork.Columns.Get(column.Id);
                    databaseColumn.Position = column.Position;
                    databaseColumn.Title = column.Title;

                    unitOfWork.Columns.Update(databaseColumn);
                }

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult DeleteBoard(string userId, int boardId)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;
                bool isUserCanDeleteBoard = CanUserDeleteBoard(userId, projectId);

                if (!isUserCanDeleteBoard)
                {
                    return GenericServiceResult.AccessDenied;
                }

                var result = DeleteTasksWithoutPermission(boardId);

                if (result != GenericServiceResult.Success)
                {
                    return GenericServiceResult.Error;
                }

                unitOfWork.BoardSettings.Delete(board.Id);
                unitOfWork.Boards.Delete(board.Id);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private GenericServiceResult DeleteTasksWithoutPermission(int boardId)
        {
            try
            {
                List<Column> columns = unitOfWork.Columns.Get(c => c.BoardId == boardId).ToList();

                foreach (var column in columns)
                {
                    List<Task> tasks = unitOfWork.Tasks.Get(t => t.ColumnId == column.Id).ToList();

                    foreach (var task in tasks)
                    {
                        unitOfWork.Attachments.Delete(a => a.TaskId == task.Id);
                        unitOfWork.Tasks.Delete(task.Id);
                    }

                    unitOfWork.Columns.Delete(column.Id);
                }

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private bool CanUserDeleteBoard(string userId, int projectId)
        {
            try
            {
                ProjectSettings projectSettings = unitOfWork.ProjectSettings.Get(projectId);
                int accessToCreateBoard = projectSettings.AccessToDeleteBoard;
                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;

                return roleUserProject <= accessToCreateBoard;
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
                ProjectSettings projectSettings = unitOfWork.ProjectSettings.Get(projectId);
                int accessToCreateBoard = projectSettings.AccessToCreateBoard;
                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;

                return roleUserProject <= accessToCreateBoard;
            }
            catch
            {
                return false;
            }
        }

        private bool CanUserChangeBoard(string userId, int boardId)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;

                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;

                if (roleUserProject == (int)ProjectRoles.Administrator)
                {
                    return true;
                }

                string creatorId = unitOfWork.Boards.Get(boardId).CreatorId;

                int roleUserBoard = creatorId == userId ? (int)ProjectRoles.Creator : roleUserProject;
                int roleToChangeBoard = unitOfWork.BoardSettings.GetFirstOrDefaultWhere(b => b.Id == boardId).AccessToChangeBoard;

                return roleUserBoard <= roleToChangeBoard;
            }
            catch
            {
                return false;
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

        private bool CanUserCreateTask(string userId, int boardId)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;

                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;

                if (roleUserProject == (int)ProjectRoles.Administrator)
                {
                    return true;
                }

                string creatorId = unitOfWork.Boards.Get(boardId).CreatorId;

                int roleUserBoard = creatorId == userId ? (int)ProjectRoles.Creator : roleUserProject;
                int roleToCreateTask = unitOfWork.BoardSettings.GetFirstOrDefaultWhere(b => b.Id == boardId).AccessToCreateTask;

                return roleUserBoard <= roleToCreateTask;
            }
            catch
            {
                return false;
            }
        }

        private bool CanUserChangeTask(string userId, int boardId, int taskId)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;

                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == projectId && p.UserId == userId).Role;

                if ((ProjectRoles)roleUserProject == ProjectRoles.Administrator)
                {
                    return true;
                }

                Task task = unitOfWork.Tasks.Get(taskId);
                int roleUserTask = userId == task.CreatorId ? (int)ProjectRoles.Creator : roleUserProject;
                int roleToChangeTask = unitOfWork.BoardSettings.Get(boardId).AccessToChangeTask;
                return roleUserTask <= roleToChangeTask;
            }
            catch
            {
                return false;
            }
        }
    }
}
