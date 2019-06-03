using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Task;

namespace SAM.Taskboard.Logic.Services
{
    public interface ITaskService
    {
        GenericServiceResult CreateTask(CreateTaskViewModel model, string userId);
        GenericServiceResult MoveTask(string userId, int boardId, int taskId, int columnId);
    }
}
