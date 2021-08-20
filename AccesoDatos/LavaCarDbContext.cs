using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Entidades;


#nullable disable

namespace AccesoDatos
{
    public partial class LavaCarDbContext : DbContext
    {
        public LavaCarDbContext()
        {
        }

        public LavaCarDbContext(DbContextOptions<LavaCarDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<Vehiculo> Vehiculos { get; set; }
        public virtual DbSet<VehiculoServicio> VehiculoServicios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=LavaCarDb;User id=sa; password=123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.HasKey(e => e.IdServicio);

                entity.Property(e => e.IdServicio).HasColumnName("ID_Servicio");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.IdVehiculo);

                entity.ToTable("Vehiculo");

                entity.Property(e => e.IdVehiculo).HasColumnName("ID_Vehiculo");

                entity.Property(e => e.Dueno)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Placa)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VehiculoServicio>(entity =>
            {
                entity.HasKey(e => e.IdVehiculoServicio);

                entity.ToTable("Vehiculo-Servicio");

                entity.Property(e => e.IdVehiculoServicio).HasColumnName("ID_Vehiculo-Servicio");

                entity.Property(e => e.IdServicio).HasColumnName("ID_Servicio");

                entity.Property(e => e.IdVehiculo).HasColumnName("ID_Vehiculo");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.VehiculoServicios)
                    .HasForeignKey(d => d.IdServicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Servicio");

                entity.HasOne(d => d.IdVehiculoNavigation)
                    .WithMany(p => p.VehiculoServicios)
                    .HasForeignKey(d => d.IdVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehiculo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
