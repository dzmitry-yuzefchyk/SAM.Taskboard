using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class UserSettings
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string PrimaryColor { get; set; } = "WHITE";

        [Required]
        [MaxLength(64)]
        public string SecondaryColor { get; set; } = "ORANGE";

        [Required]
        public bool EmailNotification { get; set; } = false;

        public User User { get; set; }
    }
}
