using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using introducao.Model;

namespace Blog.Externsions
{
    public static class RoleClaimsExtension
    {
        // pegamos o perfil do usuario e iremos transformalo em Claims
        public static IEnumerable<Claim> TransformaPerfilDoUsuarioEmClaims(this User user)
        {
            var results = new List<Claim>
            {
                new (ClaimTypes.Name, user.Email!)
            };
            // pegando todos os Perfil que o usuario tem e RETORNAMOS UM CLAIM
            results.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug!)));

            return results;
        }

    }
}