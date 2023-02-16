using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Infrastructure.Configurations;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Configurations;
using TDL.Infrastructure.Persistence.Context;

namespace TDL.Domain.Context
{
    public class ContextFactory : IContextFactory<TdlContext>
    {
        private readonly DbContextOptions<TdlContext> _contextOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnumerable<IEntityTypeConfigurationDependency> _entityTypeConfigurations;
        private readonly IOptions<AppSettings> _appSettings;

        public ContextFactory(DbContextOptions<TdlContext> contextOptions,
            IHttpContextAccessor httpContextAccessor,
            IEnumerable<IEntityTypeConfigurationDependency> entityTypeConfigurations,
            IOptions<AppSettings> appSettings)
        {
            _contextOptions= contextOptions;
            _httpContextAccessor= httpContextAccessor;
            _entityTypeConfigurations= entityTypeConfigurations;
            _appSettings= appSettings;
        }

        public TdlContext Create()
        {
            var userName = _httpContextAccessor.HttpContext.GetUserName();
            
            return new TdlContext(_contextOptions, _entityTypeConfigurations, _appSettings, userName, _httpContextAccessor.HttpContext.GetTimeZone());
        }
    }
}
