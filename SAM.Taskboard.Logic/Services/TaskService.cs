using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

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
                int taskId = task.Id;

                GenericServiceResult result = UploadAttachment(taskId, model.Attachments);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private GenericServiceResult UploadAttachment(int taskId, List<HttpPostedFileBase> attachments)
        {
            try
            {
                List<string> allowedExtensions = new List<string> { ".png", ".jpg", ".doc", ".docx", ".txt", ".xls", ".rtf", ".zip" };
                int maxAttachmentsCount = 6;
                foreach (var attachment in attachments)
                {
                    if (maxAttachmentsCount == 0)
                    {
                        break;
                    }

                    string fileExtension = Path.GetExtension(attachment.FileName);
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        continue;
                    }

                    byte[] imgData;
                    using (var reader = new BinaryReader(attachment.InputStream))
                    {
                        imgData = reader.ReadBytes(attachment.ContentLength);
                    }
                    unitOfWork.Attachments.Create(new Attachment { Document = imgData, TaskId = taskId });

                    maxAttachmentsCount--;
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
