using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AquariumService.Models
{
    public partial class automare_aac2021Context : DbContext
    {
        public automare_aac2021Context()
        {
        }

        public automare_aac2021Context(DbContextOptions<automare_aac2021Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Acuario> Acuarios { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("SERVER=gator4231.hostgator.com;PORT=3306;DATABASE=automare_aac2021;UID=automare_aacweb;PASSWORD=AAC#2021#web");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acuario>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Acuario");

                entity.HasIndex(e => e.IdAcuario, "idAcuario");

                entity.HasIndex(e => e.IdUsuario, "idUsuario");

                entity.Property(e => e.AlimentarAhora).HasColumnName("alimentarAhora");

                entity.Property(e => e.AlimentarFrec)
                    .HasColumnType("time(6)")
                    .HasColumnName("alimentarFrec");

                entity.Property(e => e.AlimentarIni)
                    .HasColumnType("time(6)")
                    .HasColumnName("alimentarIni");

                entity.Property(e => e.Alimentarprog)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("alimentarprog");

                entity.Property(e => e.IdAcuario)
                    .HasColumnType("int(8)")
                    .HasColumnName("idAcuario");

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("int(8)")
                    .HasColumnName("idUsuario");

                entity.Property(e => e.IluminacionAhora).HasColumnName("iluminacionAhora");

                entity.Property(e => e.IluminacionEstado)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("iluminacionEstado");

                entity.Property(e => e.IluminacionFrec)
                    .HasColumnType("time(6)")
                    .HasColumnName("iluminacionFrec");

                entity.Property(e => e.IluminacionIni)
                    .HasColumnType("time(6)")
                    .HasColumnName("iluminacionIni");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("name");

                entity.Property(e => e.PhActual)
                    .HasColumnType("int(4)")
                    .HasColumnName("phActual");

                entity.Property(e => e.TemperActual)
                    .HasColumnType("int(4)")
                    .HasColumnName("temperActual");

                entity.Property(e => e.TemperCalentar).HasColumnName("temperCalentar");

                entity.Property(e => e.TemperEnfriar).HasColumnName("temperEnfriar");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Acuario_ibfk_1");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PRIMARY");

                entity.ToTable("Usuario");

                entity.HasIndex(e => e.IdUsuario, "idUsuario")
                    .IsUnique();

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("int(8)")
                    .HasColumnName("idUsuario");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("type");

                entity.Property(e => e.ValidationCode)
                    .HasColumnType("int(8)")
                    .HasColumnName("validationCode");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
