namespace GestionTareas_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablaTareas : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tareas", "prioridad", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tareas", "prioridad", c => c.Int(nullable: false));
        }
    }
}
