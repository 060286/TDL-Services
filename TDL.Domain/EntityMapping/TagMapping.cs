using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Configurations;

namespace TDL.Domain.EntityMapping
{
    public class TagMapping : EntityTypeConfigurationDependency<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(tag => tag.Color).IsRequired();

            builder.Property(tag => tag.Title).IsRequired();
        }
    }
}
