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

            builder.HasMany<SubTask>(todo => todo.SubTasks)
                .WithOne(todo => todo.Todo)
                .HasForeignKey(todo => todo.TodoId)
                .IsRequired(false);

            builder.HasMany<Tag>(todo => todo.Tags)
                .WithOne(todo => todo.Todo)
                .HasForeignKey(todo => todo.TodoId)
                .IsRequired(false);
        }
    }
}
