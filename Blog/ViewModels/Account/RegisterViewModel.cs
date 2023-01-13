using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.ViewModels.Account
{
    public class RegisterViewModel
    {   
        [Required(ErrorMessage = "Nome obrigatorio")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email obrigatorio")]
        [EmailAddress(ErrorMessage = "Email é invalido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Slug obrigatorio")]
        public string Slug { get; set; } = null!;

        
    }
}