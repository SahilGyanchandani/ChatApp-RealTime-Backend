
using System.ComponentModel.DataAnnotations;

namespace Minimal_Chat_Application.ParameterModels
{
    public class UserLogin
    {
        [Required(ErrorMessage ="Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        public string Password { get; set; }



    }
}
