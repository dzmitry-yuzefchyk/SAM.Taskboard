using System.Collections.Generic;

namespace SAM.Taskboard.Model
{
    public class ProjectsViewModel : PaginationModel
    {
        public List<ProjectInfo> Projects { get; set; }
    }

    public class ProjectInfo
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string About { get; set; }
    }
}
