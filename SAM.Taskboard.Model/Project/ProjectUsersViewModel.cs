using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model.Project
{
    public class ProjectUsersViewModel : PaginationModel
    {
        [Required(ErrorMessage = "Please specify field")]
        public string UserEmailToAdd { get; set; }

        public List<UserInfo> ProjectUsers { get; set; }

        public ProjectUsersViewModel()
        {
            ProjectUsers = new List<UserInfo>();
        }
    }

    public class UserInfo
    {
        public string Id { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool CanYouDeleteUser { get; set; }
    }
}
