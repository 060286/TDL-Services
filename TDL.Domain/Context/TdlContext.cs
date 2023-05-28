using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using TDL.Domain.Entities;
using TDL.Domain.Helper;
using TDL.Infrastructure.Configurations;
using TDL.Infrastructure.Persistence.Configurations;
using TDL.Infrastructure.Persistence.Context;

namespace TDL.Domain.Context
{
    public class TdlContext : BaseDbContext
    {
        private readonly AppSettings _appSettings;

        /// <summary>
        /// This constructor is using for generationg code 1st migration
        /// </summary>
        /// <param name="contextOptions"></param>
        /// <param name="appSettings"></param>
        public TdlContext(
                DbContextOptions contextOptions,
                IOptions<AppSettings> appSettings) 
            : base(contextOptions, new List<IEntityTypeConfigurationDependency>(), string.Empty, string.Empty)
        {
            //Debugger.Launch();

            _appSettings = appSettings.Value;
        }

        public TdlContext(
                DbContextOptions contextOptions, 
                IEnumerable<IEntityTypeConfigurationDependency> entityTypeConfigurations,
                IOptions<AppSettings> appSettings,
                string username, 
                string requestedTimeZone
            ) : base(contextOptions, entityTypeConfigurations, username, requestedTimeZone)
        {
            _appSettings = appSettings.Value;
        }

        public DbSet<TodoCategory> TodoCategories { get; set; }

        public DbSet<Todo> Todos { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<SubTask> SubTasks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Workspace> Workspaces { get; set; }

        public DbSet<UserWorkspace> UserWorkspaces { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(_appSettings.SchemaSettings.TdlSchema /*"tdl_services"*/);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ExcludeEntitiesFromMigration(ModelBuilder modelBuilder)
        {
            if(MigrationHelper.IsMigrationOperationExcuting())
            {
                //Ignore Something
            }
        }
    }
}
 