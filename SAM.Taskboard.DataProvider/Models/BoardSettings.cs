using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class BoardSettings
    {
        // TODO CREATOR|ADMINISTRATOR|USER|VIEWER
        [Key]
        [ForeignKey("Board")]
        public int Id { get; set; }

        public Board Board { get; set; }

        public byte[] Background { get; set; }

        [Required]
        public string PrimaryColor { get; set; } = "WHITE";

        [Required]
        public string SecondaryColor { get; set; } = "ORANGE";

        [Required]
        public string AccessToDeleteTask { get; set; } = "Admin";

        [Required]
        public string AccessToChangeTask { get; set; } = "User";

        [Required]
        public string AccessToCreateTask { get; set; } = "User";
    }
}
