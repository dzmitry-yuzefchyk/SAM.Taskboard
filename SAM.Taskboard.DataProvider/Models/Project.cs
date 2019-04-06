using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "Project";

        public string About { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public ICollection<Board> Boards { get; set; }

        public ICollection<ProjectUser> ProjectUser { get; set; }

        public ProjectSettings Settings { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }

        public Project()
        {
            Activities = new List<Activity>();
            Boards = new List<Board>();
            ProjectUser = new List<ProjectUser>();
        }
    }
}
