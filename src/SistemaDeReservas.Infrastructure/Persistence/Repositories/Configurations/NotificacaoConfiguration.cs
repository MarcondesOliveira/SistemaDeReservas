using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDeReservas.Domain.Entities;

namespace SistemaDeReservas.Infrastructure.Persistence.Repositories.Configurations
{
    public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
    {
        public void Configure(EntityTypeBuilder<Notificacao> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Mensagem).HasColumnType("varchar(200)").IsRequired();
            builder.Property(e => e.Tipo).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Destinatario).HasColumnType("varchar(50)").IsRequired();
        }
    }
}