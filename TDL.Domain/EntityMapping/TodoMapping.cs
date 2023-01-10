using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Configurations;

namespace TDL.Domain.EntityMapping
{
    public class TodoMapping : EntityTypeConfigurationDependency<Todo>
    {
        public override void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.Property(td => td.Title).IsRequired();
        }
    }
}
