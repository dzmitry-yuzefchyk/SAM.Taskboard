using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class ProjectSettings
    {
        [Key]
        [ForeignKey("Project")]
        public int Id { get; set; }
        
        public Project Project { get; set; }

        public byte[] Background { get; set; }

        [Required]
        public string AccessToDeleteBoard { get; set; } = "Creator";

        [Required]
        public string AccessToChangeProject { get; set; } = "Creator";

        [Required]
        public string AccessToCreateBoard { get; set; } = "Creator";
    }
}
