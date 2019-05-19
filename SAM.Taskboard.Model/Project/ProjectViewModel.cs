using System.Collections.Generic;

namespace SAM.Taskboard.Model.Project
{
    public class ProjectViewModel : PaginationModel
    {
        public string ProjectTitle { get; set; }
        public List<BoardInfo> Boards { get; set; }
        public bool CanUserCreateBoar { get; set; } = false;
    }

    public class BoardInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
