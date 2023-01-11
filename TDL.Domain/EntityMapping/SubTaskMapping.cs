using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Configurations;

namespace TDL.Domain.EntityMapping
{
    public class SubTaskMapping : EntityTypeConfigurationDependency<SubTask>
    {
        public override void Configure(EntityTypeBuilder<SubTask> builder)
        {
            builder.Property(x => x.Title).IsRequired();
        }
    }
}
