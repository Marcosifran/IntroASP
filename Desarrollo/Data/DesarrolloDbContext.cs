using System;
using System.Collections.Generic;
using Desarrollo.Models;
using Microsoft.EntityFrameworkCore;

namespace Desarrollo.Data;

public partial class DesarrolloDbContext : DbContext
{
    public DesarrolloDbContext()
    {
    }

    public DesarrolloDbContext(DbContextOptions<DesarrolloDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Desarrollo;Username=postgres;Password=Cac@degato1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("equipo_pkey");

            entity.ToTable("equipo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "usuario_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Equipoid).HasColumnName("equipoid");

            entity.HasOne(d => d.Equipo).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Equipoid)
                .HasConstraintName("usuario_equipoid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
