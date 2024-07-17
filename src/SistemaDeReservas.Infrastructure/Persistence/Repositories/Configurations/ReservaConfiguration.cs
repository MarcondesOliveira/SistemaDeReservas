using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDeReservas.Domain.Entities;

namespace SistemaDeReservas.Infrastructure.Persistence.Repositories.Configurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {


            builder.HasKey(e => e.Id);
            builder.Property(e => e.Data).HasColumnType("datetime").IsRequired().HasMaxLength(100);
            builder.Property(e => e.Status).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Hora).HasColumnType("time").IsRequired();
            builder.Property(e => e.UsuarioId).HasColumnType("int").IsRequired();

            builder.HasOne(e => e.Usuario)
                .WithMany(u => u.Reservas)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
