using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Project;
using SAM.Taskboard.Model.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    unitOfWork.Attachments.Create(new Attachment
                    {
                        Document = imgData,
                        TaskId = taskId,
                        Type = attachment.ContentType,
                        Extension = fileExtension,
                        Name = attachment.FileName
                    });

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

        public OperationResult<TaskViewModel> ViewTask(string userId, int taskId)
        {
            try
            {
                Task task = unitOfWork.Tasks.Get(taskId);
                Column column = unitOfWork.Columns.Get(task.ColumnId);
                Board board = unitOfWork.Boards.Get(column.BoardId);
                int projectId = board.ProjectId;

                bool isUserHaveAccess = CanUserViewTask(userId, projectId);

                if (!isUserHaveAccess)
                {
                    return new OperationResult<TaskViewModel> { Model = null, Message = GenericServiceResult.AccessDenied };
                }

                TaskViewModel model;
                Project project = unitOfWork.Projects.Get(projectId);
                UserProfile creatorProfile = unitOfWork.ClientManager.GetProfile(task.CreatorId);
                User creator = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Id == task.CreatorId);

                string assigneeEmail = null;
                string assigneeIcon = null;

                if (task.AssigneeId != null)
                {
                    UserProfile assigneeProfile = unitOfWork.ClientManager.GetProfile(task.AssigneeId);
                    User assignee = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Id == task.AssigneeId);
                    assigneeIcon = assigneeProfile.Icon;
                    assigneeEmail = assignee.Email;
                }

                List<Attachment> attachments = unitOfWork.Attachments.Get(a => a.TaskId == taskId).ToList();
                List<AttachmentInfo> taskAttachments = new List<AttachmentInfo>();

                foreach (var attachment in attachments)
                {
                    AttachmentInfo attachmentInfo = new AttachmentInfo
                    {
                        Document = attachment.Document,
                        FileExtension = attachment.Extension,
                        FileName = attachment.Name,
                        FileType = attachment.Type
                    };

                    taskAttachments.Add(attachmentInfo);
                }

                List<Board> boards = unitOfWork.Boards.Get(b => b.ProjectId == projectId && b.Id != board.Id).ToList();
                List<BoardInfo> boardInfos = new List<BoardInfo>();

                foreach (var b in boards)
                {
                    BoardInfo boardInfo = new BoardInfo { Id = b.Id, Title = b.Title };
                    boardInfos.Add(boardInfo);
                }

                model = new TaskViewModel
                {
                    TaskId = taskId,
                    Title = task.Title,
                    Content = task.Content,
                    Priority = (Priority)task.Priority,
                    Severity = (Severity)task.Severity,
                    Type = (TaskType)task.Type,
                    Attachments = taskAttachments,
                    AssigneeEmail = assigneeEmail,
                    AssigneeIcon = assigneeIcon,
                    CreatorEmail = creator.Email,
                    CreatorIcon = creatorProfile.Icon,
                    ProjectId = projectId,
                    ProjectTitle = project.Title,
                    BoardId = board.Id,
                    BoardTitle = board.Title,
                    ColumnId = column.Id,
                    Boards = boardInfos
                };

                return new OperationResult<TaskViewModel> { Model = model, Message = GenericServiceResult.Success };
            }
            catch
            {
                return new OperationResult<TaskViewModel> { Model = null, Message = GenericServiceResult.Error };
            }
        }

        public GenericServiceResult UpdateTask(TaskViewModel model, string userId)
        {
            try
            {
                bool isUserCanChangeTask = CanUserChangeTask(userId, model.BoardId, model.TaskId);
                if (!isUserCanChangeTask)
                {
                    return GenericServiceResult.AccessDenied;
                }

                string creatorId = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Email == model.CreatorEmail).Id;
                string assigneId = null;
                if (model.AssigneeEmail != null)
                {
                    assigneId = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Email == model.AssigneeEmail).Id;
                }

                if (model.AssigneeId != null)
                {
                    assigneId = model.AssigneeId;
                }

                Task task = unitOfWork.Tasks.Get(model.TaskId);

                task.Id = model.TaskId;
                task.Title = model.Title;
                task.Content = model.Content;
                task.Priority = (int)model.Priority;
                task.Severity = (int)model.Severity;
                task.Type = (int)model.Type;
                task.ColumnId = model.ColumnId;
                task.AssigneeId = assigneId;
                task.CreatorId = creatorId;

                unitOfWork.Tasks.Update(task);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        public GenericServiceResult DeleteTask(string userId, int taskId, int boardId)
        {
            try
            {
                bool isUserCanDeleteTask = CanUserDeleteTask(userId, boardId, taskId);
                if (!isUserCanDeleteTask)
                {
                    return GenericServiceResult.AccessDenied;
                }

                unitOfWork.Attachments.Delete(a => a.TaskId == taskId);
                unitOfWork.Tasks.Delete(taskId);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }

        private bool CanUserViewTask(string userId, int projectId)
        {
            try
            {
                ProjectUser projectUser = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(p => p.ProjectId == projectId && p.UserId == userId);
                return projectUser != null;
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

        private bool CanUserDeleteTask(string userId, int boardId, int taskId)
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
                int roleToDeleteTask = unitOfWork.BoardSettings.Get(boardId).AccessToDeleteTask;
                return roleUserTask <= roleToDeleteTask;
            }
            catch
            {
                return false;
            }
        }
    }
}
