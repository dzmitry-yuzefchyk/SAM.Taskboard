using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Taskboard.Model.Board
{
    public class BoardsViewModel : PaginationModel
    {
        public List<BoardInfo> Boards { get; set; }
    }

    public class BoardInfo
    {
        public string Title { get; set; }
    }
}
