using ContatosService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContatosService.Infrastructure.Persistence.Configurations;

public sealed class ContatoConfiguration : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("contatos");

        builder.HasKey(contato => contato.Id);

        builder.Property(contato => contato.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(contato => contato.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(contato => contato.DataNascimento)
            .HasColumnName("data_nascimento")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(contato => contato.Sexo)
            .HasColumnName("sexo")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(contato => contato.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Ignore(contato => contato.EstaAtivo);

        builder.HasIndex(contato => contato.Status);
    }
}
