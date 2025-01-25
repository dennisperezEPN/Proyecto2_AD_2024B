using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace GestionTareas_backend.Configuracion
{
    public class Startup 
    {
        public void Configuration(IAppBuilder app)
        {
            // Configuración del middleware para JWT
            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            var audience = ConfigurationManager.AppSettings["JwtAudience"];
            var secret = ConfigurationManager.AppSettings["JwtSecret"];

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secret))
                }
            });
        }
    }
}