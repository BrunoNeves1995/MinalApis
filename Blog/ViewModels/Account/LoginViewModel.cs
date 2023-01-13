using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email obrigatorio")]
        [EmailAddress(ErrorMessage = "Email é invalido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Nome obrigatorio")]
        public string Password { get; set; } = null!;
    }
}