using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class User : IdentityUser
    { 
        public UserSettings Settings { get; set; }

        public UserProfile Profile { get; set; }

        public ICollection<ProjectUser> ProjectUser { get; set; }

        public ICollection<BoardUser> BoardUser { get; set; }

        public User()
        {
            ProjectUser = new List<ProjectUser>();
            BoardUser = new List<BoardUser>();
        }
    }
}
