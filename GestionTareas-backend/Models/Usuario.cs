using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionTareas_backend.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string perfil { get; set; } // admin o miembro


        // Relación 1 a muchos: Un usuario puede tener muchas tareas
        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}