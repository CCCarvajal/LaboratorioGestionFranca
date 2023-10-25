using System;
using System.Collections.Generic;
using CapaDatos;
using CapaModelos.BD;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos;

public partial class LaboratorioContext : DbContext
{
    public LaboratorioContext()
    {
    }

    public LaboratorioContext(DbContextOptions<LaboratorioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Elemento> Elementos { get; set; }

    public virtual DbSet<RelacionTecnicoElemento> RelacionTecnicoElementos { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<Tecnico> Tecnicos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;DataBase=Laboratorio;Trusted_Connection=True;Integrated Security=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Elemento>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("pkCodigoElemento");

            entity.Property(e => e.Codigo)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RelacionTecnicoElemento>(entity =>
        {
            entity
                .HasKey(r => r.Id).HasName("Id");

            entity.ToTable("RelacionTecnicoElemento");

            entity.Property(e => e.CodigoElemento)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.CodigoTecnico)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.CodigoElementoNavigation).WithMany()
                .HasForeignKey(d => d.CodigoElemento)
                .HasConstraintName("FK__RelacionT__Codig__4222D4EF");

            entity.HasOne(d => d.CodigoTecnicoNavigation).WithMany()
                .HasForeignKey(d => d.CodigoTecnico)
                .HasConstraintName("FK__RelacionT__Codig__412EB0B6");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("pkCodigoSucursal");

            entity.ToTable("Sucursal");

            entity.Property(e => e.Codigo)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tecnico>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("pkCodigoTecnico");

            entity.ToTable("Tecnico");

            entity.Property(e => e.Codigo)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.CodigoSucursal)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.SueldoBase).HasColumnType("numeric(18, 0)");

            entity.HasOne(d => d.CodigoSucursalNavigation).WithMany(p => p.Tecnicos)
                .HasForeignKey(d => d.CodigoSucursal)
                .HasConstraintName("FK__Tecnico__CodigoS__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
