using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class UserSettings
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        public User User { get; set; }

        [Required]
        public string PrimaryColor { get; set; } = "WHITE";

        [Required]
        public string SecondaryColor { get; set; } = "ORANGE";

        [Required]
        public bool EmailNotification { get; set; } = false;
    }
}
