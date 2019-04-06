using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "Team";

        public ICollection<Project> Projects { get; set; }

        public ICollection<TeamUser> TeamUser { get; set; }

        public Team()
        {
            Projects = new List<Project>();
            TeamUser = new List<TeamUser>();
        }
    }
}
