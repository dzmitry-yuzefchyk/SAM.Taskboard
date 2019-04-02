using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = "Project";

        [MaxLength(256)]
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
