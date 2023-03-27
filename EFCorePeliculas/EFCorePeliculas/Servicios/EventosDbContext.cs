using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCorePeliculas.Servicios
{
    public interface IEventosDbContext
    {
        void ManejarSaveChangeFailed(object sender, SaveChangesFailedEventArgs args);
        void ManejarSavedChanges(object sender, SavedChangesEventArgs args);
        void ManejarSavingChanges(object sender, SavingChangesEventArgs args);
        void ManejarStateChanged(object sender, EntityStateChangedEventArgs args);
        void ManejarTracked(object sender, EntityTrackedEventArgs args);
    }
    public class EventosDbContext:IEventosDbContext
    {
        private readonly ILogger<EventosDbContext> logger;

        public EventosDbContext(ILogger<EventosDbContext> logger)
        {
            this.logger = logger;
        }

        public void ManejarTracked(object sender, EntityTrackedEventArgs args)
        {
            logger.LogInformation($"Entidad: {args.Entry.Entity} estado: {args.Entry.State}");
        }

        public void ManejarStateChanged(object sender, EntityStateChangedEventArgs args)
        {
            logger.LogInformation($"Entidad: {args.Entry.Entity} estado anterior: {args.OldState}" +
                                    $" - nuevo estado {args.NewState}");
        }

        public void ManejarSavingChanges(object sender, SavingChangesEventArgs args)
        {
            var entidades = ((ApplicationDbContext)sender).ChangeTracker.Entries();

            foreach (var entidad in entidades)
            {
                logger.LogInformation($"Entidad: {entidad.Entity} va a ser {entidad.State}");
            }
        }

        public void ManejarSavedChanges(object sender, SavedChangesEventArgs args)
        {
            logger.LogInformation($"Fueron procesadas {args.EntitiesSavedCount} entidades");
        }

        public void ManejarSaveChangeFailed(object sender, SaveChangesFailedEventArgs args)
        {
            logger.LogError(args.Exception, "Error en el SaveChanges");
        }
    }
}
