using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model.Board
{
    public class CreateColumnViewModel
    {
        public int BoardId { get; set; }
        [Required(ErrorMessage = "Please specify this field")]
        [MaxLength(32, ErrorMessage = "32 symbols maximum")]
        public string Title { get; set; }
    }
}
