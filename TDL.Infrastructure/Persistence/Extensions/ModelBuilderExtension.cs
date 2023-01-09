using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace TDL.Infrastructure.Persistence.Extensions
{
    public static class ModelBuilderExtension
    {
        public static ModelBuilder EntitiesOfType<TType>(this ModelBuilder builder, Action<EntityTypeBuilder> buildAction) 
            where TType : class 
        {
            Type type = typeof(TType);

            foreach(var entityType in builder.Model.GetEntityTypes())
            {
                if (type.IsAssignableFrom(entityType.ClrType))
                    buildAction(builder.Entity(entityType.ClrType));
            }

            return builder;
        }
    }
}
