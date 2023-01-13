using System.Text;
using System.IdentityModel.Tokens.Jwt;
using introducao.Model;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Blog.Externsions;

namespace Blog.Services
{
    public class TokenService
    {
        // usuario tem o perfil dentro dele 
        public string GeradorDeToken(User user)
        {

            JwtSecurityTokenHandler ManipuladorDeToken = new();

            // convertendo a chave para um  byte[]
            var chave = Encoding.ASCII.GetBytes(Configuration.JwtKey);
            var claims = user.TransformaPerfilDoUsuarioEmClaims();

            // configurações dos token, contem todas as informações
            var ConfiguracaoToken = new SecurityTokenDescriptor
            {   
                Subject = new ClaimsIdentity(claims),

                
                // Subject = new ClaimsIdentity(new Claim[]
                // {
                //     new (ClaimTypes.Name, value: "brunoneves"),
                //     new (ClaimTypes.Role, value: "user"),
                //     new (ClaimTypes.Role, value: "admin")
                // }),
               
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chave),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            // criando o token
            var token = ManipuladorDeToken.CreateToken(ConfiguracaoToken);

            return ManipuladorDeToken.WriteToken(token);



        }
    }
}