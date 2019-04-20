using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class BoardSettings
    {
        [Key]
        [ForeignKey("Board")]
        public int Id { get; set; }

        public Board Board { get; set; }

        public byte[] Background { get; set; }

        [Required]
        public string AccessToDeleteTask { get; set; } = "Admin";

        [Required]
        public string AccessToChangeTask { get; set; } = "User";

        [Required]
        public string AccessToCreateTask { get; set; } = "User";
    }
}
