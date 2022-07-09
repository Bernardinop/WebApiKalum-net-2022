using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Aspirante> Aspirante { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<Jornada> Jornada { get; set; }
        public DbSet<ExamenAdmision> ExamenAdmision { get; set; }
        public DbSet<Inscripcion> Incripcion { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CuentaPorCobrar> CuentaPorCobrar { get; set; }
        public DbSet<ResultadoExamenAdmision> ResultadoExamenAdmision { get; set; }
        public DbSet<Inscripcion> Inscripcion { get; set; }
        public DbSet<InversionCarreraTecnica> InversionCarreraTecnica { get; set; }
        public DbSet<InscripcionPago> InscripcionPago { get; set; }
        public KalumDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new { ct.CarreraId });
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new { j.JornadaId });
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new { a.NoExpediente });
            modelBuilder.Entity<ExamenAdmision>().ToTable("ExamenAdmision").HasKey(ex => new { ex.ExamenId });
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion").HasKey(i => new { i.InscripcionId });
            modelBuilder.Entity<Alumno>().ToTable("Alumno").HasKey(a => new { a.Carne });
            modelBuilder.Entity<Cargo>().ToTable("Cargo").HasKey(c => c.CargoId);
            modelBuilder.Entity<CuentaPorCobrar>().ToTable("CuentaporCobrar").HasKey(cxc => new { cxc.Correlativo });
            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica").HasKey(ict => new { ict.InversionId });
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcionPago").HasKey(ip => new { ip.BoletaPago, ip.NoExpediente, ip.Anio });
            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision").HasKey(rea => new { rea.NoExpediente, rea.Anio });

            modelBuilder.Entity<Aspirante>()
                .HasOne<CarreraTecnica>(a => a.CarreraTecnica)
                .WithMany(ct => ct.Aspirantes)
                .HasForeignKey(a => a.CarreraId);

            modelBuilder.Entity<Aspirante>()
               .HasOne<Jornada>(a => a.Jornada)
               .WithMany(j => j.Aspirantes)
               .HasForeignKey(a => a.JornadaId);

            modelBuilder.Entity<Aspirante>()
                .HasOne<ExamenAdmision>(a => a.ExamenAdmision)
                .WithMany(ex => ex.Aspirantes)
                .HasForeignKey(a => a.ExamenId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<CarreraTecnica>(i => i.CarreraTecnica)
                .WithMany(ct => ct.Inscripciones)
                .HasForeignKey(i => i.CarreraId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Jornada>(i => i.Jornada)
                .WithMany(j => j.Inscripciones)
                .HasForeignKey(i => i.JornadaId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Alumno>(i => i.Alumno)
                .WithMany(a => a.Inscripciones)
                .HasForeignKey(i => i.Carne);

            modelBuilder.Entity<CuentaPorCobrar>()
                .HasOne<Cargo>(cxc => cxc.Cargo)
                .WithMany(c => c.CuentaPorCobrar)
                .HasForeignKey(cxc => cxc.CargoId);

            modelBuilder.Entity<CuentaPorCobrar>()
            .HasOne<Alumno>(cxc => cxc.Alumno)
            .WithMany(a => a.CuentaPorCobrar)
            .HasForeignKey(cxc => cxc.Carne);

            modelBuilder.Entity<InversionCarreraTecnica>()
                .HasOne<CarreraTecnica>(ict => ict.CarreraTecnica)
                .WithMany(c => c.InversionCarreraTecnica)
                .HasForeignKey(ict => ict.CarreraId);

            modelBuilder.Entity<InscripcionPago>()
                .HasOne<Aspirante>(ip => ip.Aspirante)
                .WithMany(a => a.InscripcionPagos)
                .HasForeignKey(ip => ip.NoExpediente);

            modelBuilder.Entity<ResultadoExamenAdmision>()
                .HasOne<Aspirante>(rea => rea.Aspirante)
                .WithMany(a => a.ResultadosExamenAdmision)
                .HasForeignKey(rea => rea.NoExpediente);

        }
    }
}