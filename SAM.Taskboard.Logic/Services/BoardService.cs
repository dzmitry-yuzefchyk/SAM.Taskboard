﻿using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Board;
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

        public OperationResult<BoardViewModel> GetBoard(string userId, int boardId)
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

                List<ColumnInfo> columnInfos = GetColumns(userId, boardId);

                BoardViewModel model = new BoardViewModel()
                {
                    CanUserChangeBoard = canUserChangeBoard,
                    CanUserCreateTask = canUserCreateTask,
                    Columns = columnInfos,
                    Title = board.Title,
                    BoardId = board.Id,
                    ProjectId = projectId,
                    ProjectTitle = projectTitle
                };

                return new OperationResult<BoardViewModel> { Model = model, Message = GenericServiceResult.Success };
            }
            catch
            {
                return null;
            }
        }

        private List<ColumnInfo> GetColumns(string userId, int boardId)
        {
            try
            {
                List<Column> columns = unitOfWork.Columns.Get(c => c.BoardId == boardId).OrderBy(c => c.Position).ToList();
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();

                foreach (var column in columns)
                {
                    List<TaskInfo> taskInfos = GetTasks(userId, column.Id, boardId);

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

        private List<TaskInfo> GetTasks(string userId, int columnId, int boardId)
        {
            try
            {
                List<Task> tasks = unitOfWork.Tasks.Get(t => t.ColumnId == columnId, t => t.Priority, "ASC").ToList();
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
                        AssigneeIcon = assigneeProfile == null ? null : assigneeProfile.Icon,
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
