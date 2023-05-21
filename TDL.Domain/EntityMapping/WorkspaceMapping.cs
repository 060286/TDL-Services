using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Configurations;

namespace TDL.Domain.EntityMapping
{
    public class WorkspaceMapping : EntityTypeConfigurationDependency<Workspace>
    {
        public override void Configure(EntityTypeBuilder<Workspace> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            builder.HasMany<Section>(ws => ws.Sections)
                .WithOne(ws => ws.Workspace)
                .HasForeignKey(ws => ws.WorkspaceId)
                .IsRequired();
        }
    }
}
