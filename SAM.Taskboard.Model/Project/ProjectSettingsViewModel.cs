using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model.Project
{
    public class ProjectSettingsViewModel
    {
        public int Id { get; set; }

        [MaxLength(64, ErrorMessage = "Maximum 64 symbols")]
        [Required(ErrorMessage = "Please specify field")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        [MaxLength(512, ErrorMessage = "Maximum 512 symbols")]
        public string About { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        public BoardSettingsRole AccessToDeleteBoard { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        public ProjectSettingsRole AccessToChangeProject { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        public BoardSettingsRole AccessToCreateBoard { get; set; }

        public bool CanUserDeleteProject { get; set; }

        public ProjectUsersViewModel ProjectUsersViewModel { get; set; }
    }
}
