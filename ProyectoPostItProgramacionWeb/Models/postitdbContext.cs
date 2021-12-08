using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProyectoPostItProgramacionWeb.Models
{
    public partial class postitdbContext : DbContext
    {
        public postitdbContext()
        {
        }

        public postitdbContext(DbContextOptions<postitdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Mazo> Mazo { get; set; }
        public virtual DbSet<Nota> Nota { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Mazo>(entity =>
            {
                entity.ToTable("mazo");

                entity.HasIndex(e => e.IdUsuario, "fk_Mazo_Usuario1_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IdUsuario).HasColumnType("int(11)");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Mazo)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkMazoUsuario");
            });

            modelBuilder.Entity<Nota>(entity =>
            {
                entity.ToTable("nota");

                entity.HasIndex(e => e.IdMazo, "fk_Nota_Mazo_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(720);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.IdMazo).HasColumnType("int(11)");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdMazoNavigation)
                    .WithMany(p => p.Nota)
                    .HasForeignKey(d => d.IdMazo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkNotaMazo");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
