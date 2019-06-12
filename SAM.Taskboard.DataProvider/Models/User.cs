using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace SAM.Taskboard.DataProvider.Models
{
    public class User : IdentityUser
    {
        public UserSettings Settings { get; set; }

        public UserProfile Profile { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Board> Boards { get; set; }

        public ICollection<ProjectUser> ProjectUser { get; set; }

        public User()
        {
            ProjectUser = new List<ProjectUser>();
        }
    }
}
