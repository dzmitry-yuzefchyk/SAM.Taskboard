using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        public User User { get; set; }

        [Required]
        public string Name { get; set; }

        public string About { get; set; }

        public string Icon { get; set; }
    }
}
