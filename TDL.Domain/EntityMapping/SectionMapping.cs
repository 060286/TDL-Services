using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Configurations;

namespace TDL.Domain.EntityMapping
{
    public class SectionMapping : EntityTypeConfigurationDependency<Section>
    {
        public override void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasMany<Todo>(ws => ws.Todos)
                .WithOne(ws => ws.Section)
                .HasForeignKey(ws => ws.SectionId)
                .IsRequired(false);
        }
    }
}
