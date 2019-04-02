using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    class ProjectSettings
    {
        [Key]
        [ForeignKey("Project")]
        public int Id { get; set; }

        [MaxLength(2048)]
        public byte[] Background { get; set; }

        [Required]
        [RegularExpression(@"CREATOR|ADMINISTRATOR|USER|VIEWER")]
        public string AccessToDeleteBoard { get; set; } = "Creator";

        [Required]
        [RegularExpression(@"CREATOR|ADMINISTRATOR|USER|VIEWER")]
        public string AccessToChangeProject { get; set; } = "User";

        [Required]
        [RegularExpression(@"CREATOR|ADMINISTRATOR|USER|VIEWER")]
        public string AccessToCreateTask { get; set; } = "User";
    }
}
