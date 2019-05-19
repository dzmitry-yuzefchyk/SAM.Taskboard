using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Board;
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

        public GenericServiceResult CreateBoard(CreateBoardViewModel model, string userId, int projectId)
        {
            try
            {
                BoardSettings boardSettings = new BoardSettings
                {
                    AccessToChangeTask = (int)model.AccessToChangeTask,
                    AccessToCreateTask = (int)model.AccessToCreateTask,
                    AccessToDeleteTask = (int)model.AccessToDeleteTask,
                    Background = null
                };

                DataProvider.Models.Board board = new DataProvider.Models.Board
                {
                    Title = model.Title,
                    Settings = boardSettings,
                    ProjectId = projectId
                };

                BoardUser boardUser = new BoardUser
                {
                    Board = board,
                    UserId = userId,
                    Role = (int)CustomRoles.Creator
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
    }
}
