using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAM.Taskboard.Model
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please specify field")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Password minimal length is 8, should contain one letter and number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirmation password and Password must match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
