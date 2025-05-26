namespace WordlessApi.Cors
{
    static class CorsSettingsExtensions
    {
        const string DEFAULT_CORS_CONFIG_PATH = "Kestrel:Cors";
        public static void AddConfiguredCors(this WebApplicationBuilder builder, string configPath = DEFAULT_CORS_CONFIG_PATH)
        {
            builder.Services.AddCors((Action<Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions>)(options =>
            {
                CorsSettings corsSettings;

                if (configPath != null)
                {
                    corsSettings = ConfigurationBinder.Get<CorsSettings>(
                                        builder.Configuration.GetSection(configPath))
                                    ?? CorsSettings.AllowAllPolicy();
                }
                else
                {
                    corsSettings = CorsSettings.AllowAllPolicy();
                }

                options.AddDefaultPolicy(
                        builder =>
                        {
                            _ = (corsSettings.AllowedOrigins.Length==0 || corsSettings.AllowedOrigins.Contains("*"))
                                ? builder.AllowAnyOrigin() : builder.WithOrigins(corsSettings.AllowedOrigins);

                            _ = (corsSettings.AllowedMethods.Length==0 || corsSettings.AllowedMethods.Contains("*"))
                                ? builder.AllowAnyMethod() : builder.WithMethods(corsSettings.AllowedMethods);

                            _ = (corsSettings.AllowedHeaders.Length==0 || corsSettings.AllowedHeaders.Contains("*"))
                                ? builder.AllowAnyHeader() : builder.WithHeaders(corsSettings.AllowedHeaders);

                            _ =  corsSettings.AllowCredentials ? builder.AllowCredentials(): builder.DisallowCredentials();
                        });
            }));
        }
    }
}
