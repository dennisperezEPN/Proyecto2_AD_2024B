using GestionTareas_backend.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GestionTareas_backend.Models;
using System.Data.Entity;

namespace GestionTareas_backend.Controllers
{
    public class TareasController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        // Obtener las tareas
        [HttpGet]
        public IEnumerable<Tarea> ObtenerTareas()
        {
            return db.Tareas.ToList();
        }

        // Obtener tarea por id
        [HttpGet]
        public IHttpActionResult ObtenerTarea(int id)
        {
            var tarea = db.Tareas.Find(id);

            if (tarea == null)
                return NotFound();

            return Ok(tarea);
        }

        // Filtrar tareas con el estado 'Pendiente'
        [HttpGet]
        [Route("api/tareas/filter")]
        public IHttpActionResult FiltrarTareasPorEstado(string estado)
        {
            // Compara el valor del estado de las tareas con el estado
            // pasado como parametro
            var tareasFiltradas = db.Tareas
                .Where(t => t.estado.Equals(estado, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!tareasFiltradas.Any())
                return NotFound();

            return Ok(tareasFiltradas);
        }


        // Crear una tarea
        [HttpPost]
        public IHttpActionResult CrearTarea(Tarea tarea)
        {
            // Se comprueba que los datos cumplan con las reglas de 
            // validacion definidas en el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Se agrega una tarea
            tarea.fechaCreacion = DateTime.Now;
            db.Tareas.Add(tarea);
            db.SaveChanges();

            // Devuelve una respuesta HTTP 201 Created
            // Incluye el recurso creado
            return CreatedAtRoute("DefaultApi", new { id = tarea.id }, tarea);


        }

        // Actualizar una tarea
        [HttpPut]
        public IHttpActionResult ActualizarTarea(int id, Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Se actualiza el recurso
            db.Entry(tarea).State = EntityState.Modified;
            db.SaveChangesAsync();

            // Devuelve una respuesta HTTP 204 No Content.
            // Indica que no hay nada que devolver en el cuerpo
            // de la respuesta
            return StatusCode(HttpStatusCode.NoContent);
        }

        // Eliminar una tarea
        [HttpDelete]
        public IHttpActionResult EliminarTarea(int id)
        {
            var tarea = db.Tareas.Find(id);

            if (tarea == null)
                return NotFound();

            db.Tareas.Remove(tarea);
            db.SaveChanges();

            return Ok(tarea);
        }
    }
}
