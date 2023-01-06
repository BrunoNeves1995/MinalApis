using Microsoft.EntityFrameworkCore;
using Tarefas.Data.DataMappings;
using Tarefas.models;

namespace Tarefas.Data
{
    public class DataContext :  DbContext
    {
        
        public DbSet<TarefaModel>? Tarefas { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"DataSource=app.db;Cache=Shared");
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TarefaMap());
        }
    }


}