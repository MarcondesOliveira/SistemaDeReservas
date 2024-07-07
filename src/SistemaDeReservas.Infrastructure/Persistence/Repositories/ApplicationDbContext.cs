using Microsoft.EntityFrameworkCore;
using SistemaDeReservas.Domain.Entities;

namespace SistemaDeReservas.Infrastructure.Persistence.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Notificacao> Notificacao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
