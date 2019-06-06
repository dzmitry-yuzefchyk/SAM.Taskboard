using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model.Profile
{
    public class ProfileSettingsViewModel
    {
        [MaxLength(64, ErrorMessage = "Maximum 64 symbols")]
        [Required(ErrorMessage = "Please specify this field")]
        public string Name { get; set; }

        public string About { get; set; }

        public string Icon { get; set; }

        public bool EmailNotifications { get; set; }

        public string Theme { get; set; }

        public string Message { get; set; }
    }
}
