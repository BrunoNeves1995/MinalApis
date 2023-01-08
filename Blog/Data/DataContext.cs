using introducao.Model;
using Microsoft.EntityFrameworkCore;
using Relacionamento.Data.DataMappings;

namespace introducao.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Category>? Categories { get; set; } = null!;
        public DbSet<Post>? Posts { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=TERMINAL01\SQLEXPRESS;Database=EFBlog;User ID=admin;Password=12345;Trusted_Connection=false;TrustServerCertificate=true");
            //  optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PostMap());
        }
    }
}