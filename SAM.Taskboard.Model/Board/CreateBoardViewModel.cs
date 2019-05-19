using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model.Board
{
    public class CreateBoardViewModel
    {
        [MaxLength(64, ErrorMessage = "Maximum 64 symbols")]
        [Required(ErrorMessage = "Please specify field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        [MaxLength(512, ErrorMessage = "Maximum 512 symbols")]
        public string About { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public CustomRoles AccessToDeleteBoard { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public CustomRoles AccessToChangeProject { get; set; }
        [Required(ErrorMessage = "Please specify field")]
        public CustomRoles AccessToCreateBoard { get; set; }
    }
}
