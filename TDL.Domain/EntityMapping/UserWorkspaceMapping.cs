using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Configurations;

namespace TDL.Domain.EntityMapping
{
    public class UserWorkspaceMapping : EntityTypeConfigurationDependency<UserWorkspace>
    {
        public override void Configure(EntityTypeBuilder<UserWorkspace> builder)
        {
            builder.HasOne(uw => uw.User)
                .WithMany(uw => uw.UserWorkspaces)
                .HasForeignKey(uw => uw.UserId)
                .IsRequired(false);

            builder.HasOne(uw => uw.Workspace)
                .WithMany(uw => uw.UserWorkspaces)
                .HasForeignKey(uw => uw.WorkspaceId)
                .IsRequired(false);
        }
    }
}
