using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model
{
    public class ResendConfirmationEmailViewModel
    {
        [Required(ErrorMessage = "Please specify field")]
        public string Email { get; set; }
    }
}
