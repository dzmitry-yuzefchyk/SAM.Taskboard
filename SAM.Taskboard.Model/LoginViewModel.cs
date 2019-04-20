using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
