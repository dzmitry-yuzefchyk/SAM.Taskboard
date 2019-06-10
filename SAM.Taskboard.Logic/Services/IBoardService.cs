using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Board;

namespace SAM.Taskboard.Logic.Services
{
    public interface IBoardService
    {
        GenericServiceResult CreateBoard(CreateBoardViewModel model, string userId, int projectId);
        OperationResult<BoardViewModel> GetBoard(string userId, int boardId, string orderBy, string direction, string search, bool assignedToMe);
        GenericServiceResult AddColumn(CreateColumnViewModel model, string userId);
        OperationResult<BoardSettingsViewModel> GetBoardSettings(string userId, int boardId);
        GenericServiceResult UpdateBoardSettings(BoardSettingsViewModel model, string userId);
        GenericServiceResult DeleteBoard(string userId, int boardId);
        GenericServiceResult DeleteColumn(string userId, int columnId);
    }
}
