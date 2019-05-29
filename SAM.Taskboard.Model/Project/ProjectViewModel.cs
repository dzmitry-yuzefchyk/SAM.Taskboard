using System.Collections.Generic;

namespace SAM.Taskboard.Model.Project
{
    public class ProjectViewModel : PaginationModel
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public bool CanUserChangeProject { get; set; }
        public List<BoardInfo> Boards { get; set; }
        public bool CanUserCreateBoard { get; set; }
    }

    public class BoardInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
