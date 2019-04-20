using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAM.Taskboard.Model
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Минимальная длина 8 символов, должен содержать одну букву и цифру")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
