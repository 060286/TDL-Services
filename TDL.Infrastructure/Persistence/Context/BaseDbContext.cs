using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TDL.Infrastructure.Persistence.Base;
using TDL.Infrastructure.Persistence.Configurations;
using TDL.Infrastructure.Persistence.Extensions;
using TDL.Infrastructure.Persistence.Translations;

namespace TDL.Infrastructure.Persistence.Context
{
    public class BaseDbContext : DbContext
    {
        private readonly string _username;
        private readonly string _requestedTimeZone;
        private readonly IEnumerable<IEntityTypeConfigurationDependency> _entityTypeConfigurations;

        protected BaseDbContext(DbContextOptions contextOptions,
            IEnumerable<IEntityTypeConfigurationDependency> entityTypeConfigurationDependencies,
            string userName, string requestedTimeZone) : base(contextOptions)
        {
            _username = userName;
            _requestedTimeZone = requestedTimeZone;
            _entityTypeConfigurations = entityTypeConfigurationDependencies;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EntitiesOfType<IDeletable>(builder =>
            {
                if (builder.Metadata.BaseType != null)
                {
                    return;
                }

                ParameterExpression clrParameter = Expression.Parameter(builder.Metadata.ClrType);
                MemberExpression isDeleted = Expression.Property(clrParameter, nameof(IDeletable.IsDeleted));
                BinaryExpression comparer = Expression.Equal(isDeleted, Expression.Constant(false));
                builder.HasQueryFilter(Expression.Lambda(comparer, clrParameter));
            });

            foreach(IEntityTypeConfigurationDependency entityTypeConfiguration in _entityTypeConfigurations)
            {
                entityTypeConfiguration.Configure(modelBuilder);
            }

            modelBuilder.Translate(_requestedTimeZone);
        }

        public override int SaveChanges(bool acceptAllChagesOnSuccess)
        {
            BeforeSaving();
            return base.SaveChanges(acceptAllChagesOnSuccess);
        }

        private void BeforeSaving()
        {
            var entries = ChangeTracker.Entries().ToList();

            foreach (var entry in entries)
            {
                if(entry.Entity is IAuditable auditableEntity)
                {
                    var now = DateTime.UtcNow;

                    switch(entry.State)
                    {
                        case EntityState.Modified:
                            auditableEntity.UpdatedAt = now;
                            auditableEntity.UpdatedBy = _username;
                            break;

                        case EntityState.Added:
                            auditableEntity.CreatedAt = now;
                            auditableEntity.UpdatedAt = now;
                            auditableEntity.CreatedBy = _username;
                            auditableEntity.UpdatedBy = _username;
                            break;

                        case EntityState.Deleted:
                            DeleteEntry(entry, now, _username);
                            break;

                        case EntityState.Detached:
                        case EntityState.Unchanged:
                        default: break;
                    }
                }
            }
        }

        private void DeleteEntry(EntityEntry entry, DateTime deletedDate, string userName)
        {
            if(!(entry is { Entity: IDeletable deletableEntity}))
            {
                return;
            }

            entry.State = EntityState.Modified;

            deletableEntity.IsDeleted = true;
            deletableEntity.DeletedAt = deletedDate;
            deletableEntity.DeletedBy = userName;
        }
    }
}
