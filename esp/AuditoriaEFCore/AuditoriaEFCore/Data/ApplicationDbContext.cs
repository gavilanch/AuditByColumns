using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuditoriaEFCore.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AuditoriaEFCore.Helpers;

namespace AuditoriaEFCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IServicioUsuarioActual servicioUsuarioActual;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServicioUsuarioActual servicioUsuarioActual)
            : base(options)
        {
            this.servicioUsuarioActual = servicioUsuarioActual ?? throw new ArgumentNullException(nameof(servicioUsuarioActual));
        }
        public DbSet<AuditoriaEFCore.Models.Persona> Personas { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcesarSalvado();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcesarSalvado()
        {
            var horaActual = DateTimeOffset.UtcNow;
            foreach (var item in ChangeTracker.Entries()
                .Where(predicate: e => e.State == EntityState.Added && e.Entity is Entidad))
            {
                var entidad = item.Entity as Entidad;
                entidad.FechaCreacion = horaActual;
                entidad.UsuarioCreacion = servicioUsuarioActual.ObtenerNombreUsuarioActual();
                entidad.FechaModificacion = horaActual;
                entidad.UsuarioModificacion = servicioUsuarioActual.ObtenerNombreUsuarioActual();
            }

            foreach (var item in ChangeTracker.Entries()
                .Where(predicate: e => e.State == EntityState.Modified && e.Entity is Entidad))
            {
                var entidad = item.Entity as Entidad;
                entidad.FechaModificacion = horaActual;
                entidad.UsuarioModificacion = servicioUsuarioActual.ObtenerNombreUsuarioActual();
                item.Property(nameof(entidad.FechaCreacion)).IsModified = false;
                item.Property(nameof(entidad.UsuarioCreacion)).IsModified = false;
            }
        }
    }
}
