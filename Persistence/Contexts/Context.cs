using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Persistence.EntitiesCotizacionSAC;

namespace Persistence.Contexts;

public partial class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }
    public virtual DbSet<Pr00ParametrizacionGeneral> Pr00ParametrizacionGenerals { get; set; }  
    public virtual DbSet<Pr09PlanCoberturaImagen> Pr09PlanCoberturaImagens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pr00ParametrizacionGeneral>(entity =>
        {
            entity.HasKey(e => e.IdParametro).HasName("PK__Pr00Para__37B016F4F997DCDA");

            entity.ToTable("Pr00ParametrizacionGeneral");

            entity.Property(e => e.Ambiente)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoDato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("String");
            entity.Property(e => e.UsuarioActualizacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Valor)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pr09PlanCoberturaImagen>(entity =>
        {
            entity.HasKey(e => new { e.CodigoCobertura, e.CodigoPlan }).HasName("PkPr09");

            entity.ToTable("Pr09PlanCoberturaImagen");

            entity.Property(e => e.CodigoCobertura).ValueGeneratedOnAdd();
            entity.Property(e => e.CodigoPlan)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreImagen)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(1500)
                .IsUnicode(false);
        });

    }

}


