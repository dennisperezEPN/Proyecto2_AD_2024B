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
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Web.Http;
using System.Diagnostics;
using System.IO;

namespace GestionTareas_backend.Configuracion
{
    public class Startup 
    {
        public void Configuration(IAppBuilder app)
        {
            // Configurar autenticación antes de registrar WebAPI
            ConfigureAuth(app);

            // Registrar rutas de la API
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        // Atutenticacion con JWT
        private void ConfigureAuth(IAppBuilder app)
        {
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
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secret)),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", // Esto asegura que se usa el claim 'role' para validar roles
                    ClockSkew = TimeSpan.Zero // Elimina cualquier tolerancia para la validación del tiempo
                }
            });
        }
    }
}