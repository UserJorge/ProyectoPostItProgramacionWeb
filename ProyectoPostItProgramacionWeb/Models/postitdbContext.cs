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

        public virtual DbSet<Postit> Postit { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;password=_M0r3nR0TLMR?;database=postitdb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Postit>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.IdUsuario })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("postit");

                entity.HasIndex(e => e.IdUsuario, "fk_PostIt_Usuario_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdUsuario).HasColumnType("int(11)");

                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasMaxLength(720);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Postit)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkPostItUsuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ClaveAcceso)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
