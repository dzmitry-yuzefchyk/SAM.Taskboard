using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string About { get; set; }

        [MaxLength(2048)]
        public byte[] Icon { get; set; }

        public User User { get; set; }
    }
}
