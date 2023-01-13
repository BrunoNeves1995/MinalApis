using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public static class Configuration
    {
        public static string JwtKey  ="30ee5d5a47f93087668935ce11d38da359b765b787ebaf543f3955dc4e7a82827d829514e1dd9ae1e973e1a0d71a98456d3be20837076beac0b9e7148ca442ad=";
        public static string ApiKeyName="api_name";
        public static string ApiKey="curso_api_!WH3V1jsH;(oU0rCJ5*Fi0kUzDr*bcyW===";
        public static SmtpConfiguration Smtp = new();



        public class SmtpConfiguration
        {
            public string Host { get; set; } = null!;        
            public int Port { get; set; } = 25;   
            public string Dominio { get; set; } = null!;   

             public string UserName { get; set; } = null!;       
             public string Password { get; set; } = null!;       
        }

    }
}