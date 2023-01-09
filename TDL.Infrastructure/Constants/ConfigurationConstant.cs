namespace TDL.Infrastructure.Constants
{
    public static class ConfigurationConstant
    {
        public const string CorsPolicy = nameof(CorsPolicy);
        public const string AllowedOrigins = "Cors:AllowedOrigins";
        public const string DefaultContentType = "application/json";
        public const string DefaultApiVersion = "1.0";
        public const string DefaultConnection = "ConnectionString:DefaultConnection";
        public const string TdlSchemaName = "AppSettings:SchemaSettings:TdlSchema";
        public const string TimeZoneKey = "X-Timezone-Offset";
        public const string HealthCheckPath = "/health";
        public const string LoggingSection = "Logging";
        public const string SwaggerAuthorizationDescription = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {Token}\"";
        public const string SwaggerSettings = nameof(SwaggerSettings);
        public const string AzureADAuthority = "AppSettings:AzureAdAuthentication:Authority";
        public const string AzureADTenantId = "AppSettings:AzureAdAuthentication:TenantId";
        public const string AzureADAudience = "AppSettings:AzureAdAuthentication:Audience";
    }
}
