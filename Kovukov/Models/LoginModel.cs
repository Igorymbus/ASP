using System.ComponentModel.DataAnnotations;

namespace Kovukov.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string email { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]


        [DataType(DataType.Password)]

    public string passwords { get; set; }
    }
}
