using EFCorePeliculas.Entidades;
using EFCorePeliculas.Entidades.Configuraciones;
using EFCorePeliculas.Entidades.Seeding;
using EFCorePeliculas.Entidades.SinLlave;
using EFCorePeliculas.Servicios;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCorePeliculas
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IServicioUsuario servicioUsuario;

        public ApplicationDbContext(DbContextOptions options, IServicioUsuario servicioUsuario, IEventosDbContext eventosDbContext) : base(options)
        {
            this.servicioUsuario = servicioUsuario;

            if(eventosDbContext is not null )
            {
                //ChangeTracker.StateChanged += eventosDbContext.ManejarStateChanged;
                //ChangeTracker.Tracked += eventosDbContext.ManejarTracked;
                SavingChanges += eventosDbContext.ManejarSavingChanges;
                SavedChanges += eventosDbContext.ManejarSavedChanges;
                SaveChangesFailed += eventosDbContext.ManejarSaveChangeFailed;
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new ActorConfig());
            //modelBuilder.ApplyConfiguration(new CineConfig());
            //modelBuilder.ApplyConfiguration(new GeneroConfig());
            //modelBuilder.ApplyConfiguration(new PeliculaActorConfig());
            //modelBuilder.ApplyConfiguration(new PeliculaConfig());
            //modelBuilder.ApplyConfiguration(new SalaDeCineConfig());

            //modelBuilder.Entity<Log>().Property(l => l.Id).ValueGeneratedNever();

            //modelBuilder.Ignore<Direccion>();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedingModuloConsulta.Seed(modelBuilder);
            SeedingPersonaMensaje.Seed(modelBuilder);

            modelBuilder.Entity<CineSinUbicacion>().HasNoKey().ToSqlQuery("Select Id, Nombre From Cines").ToView(null);

            modelBuilder.Entity<PeliculaConConteos>().HasNoKey().ToView("PeliculasConConteos");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if(property.ClrType == typeof(string) && property.Name.Contains("URL", StringComparison.CurrentCultureIgnoreCase))
                    {
                        property.SetIsUnicode(false);
                        property.SetMaxLength(500);
                    }
                }
            }
            modelBuilder.Entity<PeliculaAlquilable>().ToTable("PeliculasAlquilables");
            modelBuilder.Entity<Merchandising>().ToTable("Merchandising");

            var pelicula1 = new PeliculaAlquilable()
            {
                Id = 1,
                Nombre = "Spider-Man",
                PeliculaId = 1,
                Precio = 5.99m
            };

            var merch1 = new Merchandising()
            {
                Id = 2,
                DisponibleEnInventario = true,
                EsRopa = true,
                Nombre = "T-Shirt One Piece",
                Peso = 1,
                Volumen = 1,
                Precio = 11
            };

            modelBuilder.Entity<PeliculaAlquilable>().HasData(pelicula1);
            modelBuilder.Entity<Merchandising>().HasData(merch1);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcesarSalvado();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcesarSalvado()
        {
            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is EntidadAuditable))
            {
                var entidad = item.Entity as EntidadAuditable;
                entidad.UsuarioModificacion = servicioUsuario.ObtenerUsuarioId();
                entidad.UsuarioCreacion = servicioUsuario.ObtenerUsuarioId();
            }

            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified && e.Entity is EntidadAuditable))
            {
                var entidad = item.Entity as EntidadAuditable;
                entidad.UsuarioModificacion = servicioUsuario.ObtenerUsuarioId();
                item.Property(nameof(entidad.UsuarioCreacion)).IsModified = false; 
            }
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<CineOferta> CinesOfertas { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<CineSinUbicacion> CinesSinUbicacion { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<CineDetalle> CineDetalle { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }
    }
}
