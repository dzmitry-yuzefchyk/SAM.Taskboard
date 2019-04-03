using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class BoardSettings
    {
        [Key]
        [ForeignKey("Board")]
        public int Id { get; set; }

        [MaxLength(2048)]
        public byte[] Background { get; set; }

        [Required]
        [MaxLength(64)]
        public string PrimaryColor { get; set; } = "WHITE";

        [Required]
        [MaxLength(64)]
        public string SecondaryColor { get; set; } = "ORANGE";

        [Required]
        [RegularExpression(@"CREATOR|ADMINISTRATOR|USER|VIEWER")]
        public string AccessToDeleteTask { get; set; } = "Admin";

        [Required]
        [RegularExpression(@"CREATOR|ADMINISTRATOR|USER|VIEWER")]
        public string AccessToChangeTask { get; set; } = "User";

        [Required]
        [RegularExpression(@"CREATOR|ADMINISTRATOR|USER|VIEWER")]
        public string AccessToCreateTask { get; set; } = "User";
    }
}
