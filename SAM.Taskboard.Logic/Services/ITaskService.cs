using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Task;

namespace SAM.Taskboard.Logic.Services
{
    public interface ITaskService
    {
        GenericServiceResult CreateTask(CreateTaskViewModel model, string userId);
        GenericServiceResult MoveTask(string userId, int boardId, int taskId, int columnId);
        OperationResult<TaskViewModel> ViewTask(string userId, int taskId);
        GenericServiceResult UpdateTask(TaskViewModel model, string userId);
        GenericServiceResult DeleteTask(string userId, int taskId, int boardId);
        OperationResult<AttachmentInfo> GetAttachment(string userId, int attachmentId, int projectId);
    }
}
