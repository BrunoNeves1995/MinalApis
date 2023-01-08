using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class EditorCategoryViewModel
    {   
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Slug é obrigatório")]
        public string Slug { get; set; } = null!;
    }
}