using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDL.Infrastructure.Persistence.Configurations
{
    public abstract class EntityTypeConfigurationDependency<TEntity>
        : IEntityTypeConfigurationDependency, IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);

        public void Configure(ModelBuilder modelBuilder) => Configure(modelBuilder.Entity<TEntity>());
    }

    public interface IEntityTypeConfigurationDependency
    {
        void Configure(ModelBuilder builder);
    }
}
