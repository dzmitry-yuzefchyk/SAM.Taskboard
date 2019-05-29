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

        [Required]
        public int AccessToDeleteTask { get; set; }

        [Required]
        public int AccessToChangeTask { get; set; }

        [Required]
        public int AccessToCreateTask { get; set; }
    }
}
