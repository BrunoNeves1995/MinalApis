using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Externsions
{
    // Classe de extensão [ModelState.Values]
    public static class ModelStateExtension
    {
        // Esperamos como parametro o tipo do [ModelState] que é um [ModelStateDictionary]
        public static List<String> GetErrors(this ModelStateDictionary modelStateDictionary)
        {

            List<String> results = new();

            // percorrendo os valores
            foreach (var itemErros in modelStateDictionary.Values)
            {   
                 // percorrendo os erros
                results.AddRange(itemErros.Errors.Select(erro => erro.ErrorMessage));
            }
            return results;

            // // percorrendo os valores
            // foreach (var itemErros in modelStateDictionary.Values)
            // {
            //     // percorrendo os erros
            //     foreach (var erro in itemErros.Errors)
            //     {
            //         results.Add(erro.ErrorMessage);
            //     }
            // }
            // return results;
        }
    }
}