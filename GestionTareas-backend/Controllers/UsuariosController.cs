using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using GestionTareas_backend.DAL;
using GestionTareas_backend.Models;
using GestionTareas_backend.Services;
using System.Security.Claims;


namespace GestionTareas_backend.Controllers
{
    public class UsuariosController : ApiController
    {
        private AppDbContext bd = new AppDbContext();

        // Solicitudes para obtener usuarios
        // [Authorize]
        [HttpGet]
        public IEnumerable<Usuario> ObtenerUsuarios()
        {
            return bd.Usuarios.ToList();
        }

        [HttpGet]
        public IHttpActionResult ObtenerUsuario(int id)
        {
            var usuario = bd.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // Solicitud para crear un usuario
        //[Authorize(Roles = "admin")]
        [HttpPost]
        public IHttpActionResult CrearUsuario(Usuario usuario)
        {
            // Se comprueba que los datos cumplan con las reglas de 
            // validacion definidas en el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Se agrega el usuario
            string hashedPassword = Hasher.HashPassword(usuario.password);
            usuario.password = hashedPassword; // Se guarda el hash de la contrasena en la BD
            bd.Usuarios.Add(usuario);
            bd.SaveChanges();

            // Devuelve una respuesta HTTP 201 Created
            // Incluye el recurso creado
            return CreatedAtRoute("DefaultApi", new {id = usuario.id}, usuario);
        }

        // Solicitud para modificar un usuario
        [HttpPut]
        public IHttpActionResult ActualizarUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Comprueba que el id de la ruta de la solicitud, coincida con
            // id del objeto enviado en el cuerpo
            if (id != usuario.id)
                return BadRequest();

            // Se actualiza el recurso
            bd.Entry(usuario).State = EntityState.Modified;
            bd.SaveChangesAsync();

            // Devuelve una respuesta HTTP 204 No Content.
            // Indica que no hay nada que devolver en el cuerpo
            // de la respuesta
            return StatusCode(HttpStatusCode.NoContent);

        }

        // Solicitud para borrar un usuario
        [HttpDelete]
        public IHttpActionResult EliminarUsuario (int id)
        {
            var usuario = bd.Usuarios.Find(id);

            if (usuario == null)
                return NotFound();

            bd.Usuarios.Remove(usuario);
            bd.SaveChanges();
            
            return Ok(usuario);
        }
    }
}
