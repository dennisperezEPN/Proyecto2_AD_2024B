using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionTareas_backend.Models
{
    public class Tarea
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaPlazo { get; set; }
        public DateTime? fechaFinalizacion { get; set; }
        public string prioridad { get; set; } // baja, media, alta
        public string estado { get; set; } // pendiente, en progreso, completada
        public string comentarios { get; set; }

        // Foreign Key a Usuario (Relación 1 a N)
        public int? usuarioId { get; set; } // opcional
        public Usuario usuario { get; set; }
    }
}