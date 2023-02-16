using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Configurations;

namespace TDL.Domain.EntityMapping
{
    public class TodoCategoryMapping : EntityTypeConfigurationDependency<TodoCategory>
    {
        public override void Configure(EntityTypeBuilder<TodoCategory> builder)
        {
            builder.Property(td => td.Title).IsRequired();

            builder.HasIndex(nameof(TodoCategory.Title));

            builder.HasMany<Todo>(x => x.Todos)
                .WithOne(x => x.TodoCategory)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
