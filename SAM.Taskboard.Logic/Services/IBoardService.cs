using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Board;

namespace SAM.Taskboard.Logic.Services
{
    public interface IBoardService
    {
        GenericServiceResult CreateBoard(CreateBoardViewModel model, string userId, int projectId);
        OperationResult<BoardViewModel> GetBoard(string userId, int boardId);
        GenericServiceResult AddColumn(CreateColumnViewModel model, string userId);
    }
}
