using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Data.Configurations
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            HandleSoftDeletes(eventData?.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            HandleSoftDeletes(eventData?.Context);
            return base.SavingChanges(eventData, result);
        }

        private static void HandleSoftDeletes(DbContext? context)
        {
            if (context == null) return;

            // Find all entries marked as deleted and implement ISoftDeletable
            foreach (var entry in context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDeletable))
            {
                var softDeletableEntity = (ISoftDeletable)entry.Entity;

                // Change state to modified and mark as deleted
                entry.State = EntityState.Modified;
                softDeletableEntity.IsDeleted = true;
            }
        }
    }
}