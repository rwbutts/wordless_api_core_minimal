namespace WordlessApi.Cors
{
    static class CorsSettingsExtensions
    {
        const string DEFAULT_CORS_CONFIG_PATH = "Kestrel:Cors";
        public static void AddConfiguredCors(this WebApplicationBuilder builder, string configPath = DEFAULT_CORS_CONFIG_PATH)
        {
            builder.Services.AddCors((Action<Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions>)(options =>
            {
                CorsSettings corsPolicy;

                if (configPath != null)
                {
                    corsPolicy = ConfigurationBinder.Get<CorsSettings>(
                                        builder.Configuration.GetSection(configPath))
                                    ?? CorsSettings.AllowAllPolicy();
                }
                else
                {
                    corsPolicy = CorsSettings.AllowAllPolicy();
                }

                options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.WithOrigins(corsPolicy.AllowedOrigins ?? CorsSettings.EMPTYARRAY)
                                .WithMethods(corsPolicy.AllowedMethods ?? CorsSettings.EMPTYARRAY)
                                .WithHeaders(corsPolicy.AllowedHeaders ?? CorsSettings.EMPTYARRAY);

                            if (corsPolicy.AllowCredentials)
                            {
                                builder.AllowCredentials();
                            }
                            else
                            {
                                builder.DisallowCredentials();
                            }
                        });
            }));
        }
    }
}
