using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuditEFCore.Models;
using AuditEFCore.Helpers;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace AuditEFCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly ICurrentUserService currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }
        public DbSet<AuditEFCore.Models.Person> People { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessSave();  
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessSave()
        {
            var currentTime = DateTimeOffset.UtcNow;
            foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is Entity))
            {
                var entidad = item.Entity as Entity;
                entidad.CreatedDate = currentTime;
                entidad.CreatedByUser = currentUserService.GetCurrentUsername();
                entidad.ModifiedDate = currentTime;
                entidad.ModifiedByUser = currentUserService.GetCurrentUsername();
            }

            foreach (var item in ChangeTracker.Entries()
                .Where(predicate: e => e.State == EntityState.Modified && e.Entity is Entity))
            {
                var entidad = item.Entity as Entity;
                entidad.ModifiedDate = currentTime;
                entidad.ModifiedByUser = currentUserService.GetCurrentUsername();
                item.Property(nameof(entidad.CreatedDate)).IsModified = false;
                item.Property(nameof(entidad.CreatedByUser)).IsModified = false;
            }
        }
    }
}
