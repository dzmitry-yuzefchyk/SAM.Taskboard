using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Taskboard.Model.Board
{
    public class BoardsViewModel : PaginationModel
    {
        public List<Board> Boards { get; set; }
    }

    public class Board
    {
        public string Title { get; set; }
    }
}
