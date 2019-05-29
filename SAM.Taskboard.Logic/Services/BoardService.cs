using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Board;
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

        public BoardViewModel GetBoard(string userId, int boardId)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;

                int roleProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;
                int roleBoard = unitOfWork.BoardUser.GetFirstOrDefaultWhere(x => x.UserId == userId && x.BoardId == boardId).Role;

                int roleToChangeProject = unitOfWork.ProjectSettings.GetFirstOrDefaultWhere(x => x.Id == projectId).AccessToChangeProject;
                int roleToCreateTask = unitOfWork.BoardSettings.GetFirstOrDefaultWhere(b => b.Id == boardId).AccessToCreateTask;

                bool canUserChangeProject = roleProject <= roleToChangeProject;
                bool canUserCreateTask = roleBoard <= roleToCreateTask;

                List<Column> columns = unitOfWork.Columns.Get(c => c.BoardId == boardId).OrderBy(c => c.Position).ToList();
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();

                foreach (var column in columns)
                {
                    List<Task> tasks = unitOfWork.Tasks.Get(t => t.ColumnId == column.Id).ToList();
                    List<TaskInfo> taskInfos = new List<TaskInfo>();

                    foreach (var task in tasks)
                    {
                        taskInfos.Add(new TaskInfo
                        {
                            Id = task.Id,
                            Title = task.Title,
                            CreationTime = task.StartTime
                        });
                    }

                    columnInfos.Add(new ColumnInfo
                    {
                        Id = column.Id,
                        Title = column.Title,
                        Tasks = taskInfos
                    });
                }

                BoardViewModel model = new BoardViewModel
                {
                    CanUserChangeProject = canUserChangeProject,
                    CanUserCreateTask = canUserCreateTask,
                    Columns = columnInfos,
                    Title = board.Title,
                    BoardId = board.Id
                };

                return model;
            }

            //TODO: Error handling
            catch (SystemException e)
            {
                return null;
            }
        }

        public GenericServiceResult CreateBoard(CreateBoardViewModel model, string userId, int projectId)
        {
            try
            {
                ProjectUser projectUser = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == projectId && p.UserId == userId);
                CustomRoles userBoardRole = projectUser == null ? CustomRoles.Creator : CustomRoles.Administrator;

                BoardSettings boardSettings = new BoardSettings
                {
                    AccessToChangeTask = (int)model.AccessToChangeTask,
                    AccessToCreateTask = (int)model.AccessToCreateTask,
                    AccessToDeleteTask = (int)model.AccessToDeleteTask,
                };

                Board board = new Board
                {
                    Title = model.Title,
                    Settings = boardSettings,
                    ProjectId = projectId
                };

                BoardUser boardUser = new BoardUser
                {
                    Board = board,
                    UserId = userId,
                    Role = (int)userBoardRole
                };
                unitOfWork.BoardUser.Create(boardUser);

                return GenericServiceResult.Success;
            }

            //TODO: Error handling
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult AddColumn(CreateColumnViewModel model)
        {
            try
            {
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

        public bool IsUserHaveAccess(string userId, int boardId)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;
                return unitOfWork.ProjectUser.GetFirstOrDefaultWhere(u => u.UserId == userId && u.ProjectId == projectId) != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
