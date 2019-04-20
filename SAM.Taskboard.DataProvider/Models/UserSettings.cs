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
        public bool EmailNotification { get; set; } = false;
    }
}
