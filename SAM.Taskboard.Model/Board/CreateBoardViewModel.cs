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
        public TaskSettingsRole AccessToDeleteTask { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public TaskSettingsRole AccessToChangeTask { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public TaskSettingsRole AccessToCreateTask { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public TaskSettingsRole AccessToChangeBoard { get; set; }
    }
}
