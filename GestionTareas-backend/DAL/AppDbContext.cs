using GestionTareas_backend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace GestionTareas_backend.DAL
{
    public class AppDbContext : DbContext
    {
        // Representan tablas de la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarea> Tareas { get; set; }

    }

}