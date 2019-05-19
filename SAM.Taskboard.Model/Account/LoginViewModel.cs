using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please specify field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
