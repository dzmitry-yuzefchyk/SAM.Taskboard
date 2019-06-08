using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Task;
using System;
using System.IO;

namespace SAM.Taskboard.Logic.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public GenericServiceResult CreateTask(CreateTaskViewModel model, string userId)
        {
            try
            {
                bool isUserCanCreateTask = CanUserCreateTask(userId, model.BoardId);
                if (!isUserCanCreateTask)
                {
                    return GenericServiceResult.AccessDenied;
                }

                Task task = new Task
                {
                    ColumnId = model.ColumnId,
                    Title = model.Title,
                    Content = model.Content,
                    Priority = (int)model.Priority,
                    Severity = (int)model.Severity,
                    Type = (int)model.Type,
                    StartTime = DateTime.Now,
                    AssigneeId = model.AssigneeId,
                    CreatorId = userId
                };

                unitOfWork.Tasks.Create(task);

                foreach (var attachment in model.Attachments)
                {
                    byte[] imgData;
                    using (var reader = new BinaryReader(attachment.InputStream))
                    {
                        imgData = reader.ReadBytes(attachment.ContentLength);
                    }
                    unitOfWork.Attachments.Create(new Attachment { Document = imgData, TaskId = task.Id });
                }

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult MoveTask(string userId, int boardId, int taskId, int columnId)
        {
            try
            {
                bool isUserCanChangeTask = CanUserChangeTask(userId, boardId, taskId);
                if (!isUserCanChangeTask)
                {
                    return GenericServiceResult.AccessDenied;
                }

                Task task = unitOfWork.Tasks.Get(taskId);
                task.ColumnId = columnId;
                unitOfWork.Tasks.Update(task);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        //public bool CanUserChangeTask(string userId, int boardId)
        //{
        //    try
        //    {
        //        BoardSettings boardSettings = unitOfWork.BoardSettings.Get(boardId);
        //        Board board = unitOfWork.Boards.Get(boardId);
                
        //        int userProjectRole = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == board.ProjectId && p.UserId == userId).Role;
        //        int accessToChangeTask = boardSettings.AccessToChangeTask;

        //        if ((CustomRoles)userProjectRole == CustomRoles.Administrator)
        //        {
        //            return true;
        //        }

        //        string creatorId = board.CreatorId;
        //        int userAccessToTask = creatorId == userId ? (int)CustomRoles.Creator : (int)CustomRoles.Member;

        //        return userAccessToTask >= accessToChangeTask;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        private bool CanUserCreateTask(string userId, int boardId)
        {
            try
            {
                Board board = unitOfWork.Boards.Get(boardId);
                int projectId = board.ProjectId;
                BoardSettings boardSettings = unitOfWork.BoardSettings.Get(boardId);

                int roleUserProject = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == projectId && p.UserId == userId).Role;

                if ((ProjectRoles)roleUserProject == ProjectRoles.Administrator)
                {
                    return true;
                }

                int accessToCreateTask = boardSettings.AccessToCreateTask;
                int userAccessToBoard = userId == board.CreatorId ? (int)ProjectRoles.Creator : roleUserProject;

                return userAccessToBoard <= accessToCreateTask;
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
