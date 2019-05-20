using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model.Board
{
    public class CreateBoardViewModel
    {
        public int ProjectId { get; set; }
        [MaxLength(64, ErrorMessage = "Maximum 64 symbols")]
        [Required(ErrorMessage = "Please specify field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public CustomRoles AccessToDeleteTask { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public CustomRoles AccessToChangeTask { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public CustomRoles AccessToCreateTask { get; set; }
    }
}
