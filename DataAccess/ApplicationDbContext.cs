using EsteticaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata; 


namespace EsteticaApp.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Estilista> Estilistas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<ServicioCorte> ServiciosCorte { get; set; }
        public DbSet<ServicioTinte> ServiciosTinte { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<TransaccionPago> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Servicio>()
                .HasDiscriminator<string>("TipoServicio")
                .HasValue<ServicioCorte>("Corte")
                .HasValue<ServicioTinte>("Tinte");

            modelBuilder.Entity<Servicio>()
                .Property(s => s.Precio);
            //.HasColumnType("decimal(18,2)");

            modelBuilder.Entity<TransaccionPago>()
                .Property(t => t.MontoTotal);
                //.HasColumnType("decimal(18,2)");
        }
    }
}