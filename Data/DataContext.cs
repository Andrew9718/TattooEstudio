using CoolTattooApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolTattooApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Tatuador> Tatuador { get; set; }
        public DbSet<ImagenesTattoo> Imagenes { get; set; }
        public DbSet<Publicidad> Publicidad { get; set; }
        public DbSet<Clientes> Cliente { get; set; }
        public DbSet<Bisuteria> Bisuteria { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Relación entre Clientes y Tatuador
            builder.Entity<Clientes>()
                .HasOne(c => c.Tatuador)
                .WithMany(t => t.Clientes)
                .HasForeignKey(c => c.TatuadorId)
                .IsRequired(false);

            // Relación entre ImagenesTattoo y Tatuador
            builder.Entity<ImagenesTattoo>()
                .HasOne(i => i.Tatuador)
                .WithMany(t => t.ImagenesTattoo)
                .HasForeignKey(i => i.TatuadorId)
                .IsRequired();

        }
    }
}
