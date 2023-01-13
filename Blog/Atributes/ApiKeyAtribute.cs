using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Blog.Atributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAtribute : Attribute, IAsyncActionFilter
    {
        // verifica se existe a chave de acesso na requisição 
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context, 
            ActionExecutionDelegate next
            )
        {   
            // tentando obter um valor da nossa QueryString
           if(!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extraidoChaveApi))
           {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api não encontrada"
                };
                return;
           }

            //  se a chave nao for igual
           if(!Configuration.ApiKey.Equals(extraidoChaveApi))
           {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Acesso não autorizado"
                };
                return;
           }

           await next();
        }
    }
}