using GestionTareas_backend.Services;
using GestionTareas_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GestionTareas_backend.DAL;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GestionTareas_backend.Controllers
{
    [RoutePrefix("api/autenticacion")]
    public class AutenticacionController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            // Se obtiene los parametros del body
            string email = request.email;
            string password = request.password;

            // Validar las credenciales del usuario
            var usuario = db.Usuarios.FirstOrDefault(u => u.email == email);

            if (usuario == null)
            {
                return NotFound();
            }

            bool verifiedPassword = Hasher.VerifyPassword(password, usuario.password);

            if (!verifiedPassword)
                return Unauthorized();

            // Crear los claims 
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.email), // Nombre de usuario
                new Claim(ClaimTypes.Role, usuario.perfil) // Rol del usuario
            };

            // Genera el token
            var key = new SymmetricSecurityKey(Convert.FromBase64String(ConfigurationManager.AppSettings["JwtSecret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: ConfigurationManager.AppSettings["JwtIssuer"],
                audience: ConfigurationManager.AppSettings["JwtAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        
    }
}
