using Microsoft.Extensions.Options;

namespace WordlessApi.Config
{
    static class SettingsExtensions
    {
        const string DEFAULT_API_CONFIG_PATH = "WordlessApi";
        const string DEFAULT_WEBROOT_PATH = "wwwroot";

        /// <summary>
        /// Reads an ApiSettings object from the "WordlessApi" key in AppSettings.json, adds it as a 
        /// singleton service, initializes a builder with ContentRootPath and WebRootPath options 
        /// reflecting the ApiSettings values (if present).
        /// </summary>
        /// <param name="args">Commandline arguments to be passed to the builder</param>
        /// <param name="configPath">Key path in AppSettings.json where configuration values reside.  
        /// Defaults to "WordlessApi".  (Separate path elements with ":")</param>
        /// <returns>The builder with ContentRootPath and WebRootPath configured from appsettings.json,
        /// if necesary, and the ApiSettings object registered in DI as a singleton.</returns>
        public static WebApplicationBuilder CreateCustomApiBuilder(string[] args, string configPath = DEFAULT_API_CONFIG_PATH)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var configSection = configuration.GetSection(configPath);
            var apiSettings = configSection.Get<ApiSettings>() ?? ApiSettings.Default;

            var contentRootPath = Coalesce(apiSettings.ContentRootPath, Directory.GetCurrentDirectory());
            var webRootPath = Coalesce(apiSettings.WebRootPath, Path.Join(contentRootPath, DEFAULT_WEBROOT_PATH));

            // now we can pass those options to the real builder
            WebApplicationOptions opts = new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = contentRootPath,
                WebRootPath = webRootPath
            };

            var builder = WebApplication.CreateBuilder(opts);
            builder.Services.Configure<ApiSettings>(configSection);

            return builder;
        }

        /// <summary>
        /// Extension method added to WebApplication that checks the WordlessApi:PathBase value
        /// in AppSettings.json and, if set, adds the configured PathBase middleware to the pipeline.
        /// </summary>
        /// <param name="app">The WebApplication instance created by the WebApplicationBuilder in Program.cs</param>

        public static void ConfigureApiPathBase(this WebApplication app)
        {
            var apiSettings = app.Services.GetRequiredService<IOptions<ApiSettings>>().Value;

            if (!String.IsNullOrEmpty(apiSettings.PathBase))
            {
                app.UsePathBase(apiSettings.PathBase);
            }
        }

        /// <summary>
        /// Reads the WordlessApi:ApiRootUri value from Appsettings.json and, if set,
        /// returns a MapGroup with that parent Uri.  If the setting empty or missing,
        /// the Mapgroup is set to "/" and API endpoints are top-level site urls.
        /// </summary>
        /// <param name="app">The WebApplication instance created by the WebApplicationBuilder in Program.cs</param>
        /// <returns>RouteGroupBuilder instance for further mappings.</returns>
        public static RouteGroupBuilder CreateApiRouteGroup(this WebApplication app)
        {
            var apiSettings = app.Services.GetRequiredService<IOptions<ApiSettings>>().Value;
            var apiRootUri = !String.IsNullOrEmpty(apiSettings.ApiRootUri) ? apiSettings.ApiRootUri : "/";

            return app.MapGroup(apiRootUri);
        }

        private static string? Coalesce(string? S, string? fallback = null)
        {
            return String.IsNullOrEmpty(S) ? fallback : S;
        }

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
                                    ?? CorsSettings.CreateAllowAllPolicy();
                }
                else
                {
                    corsSettings = CorsSettings.CreateAllowAllPolicy();
                }

                corsSettings.Validate();

                options.AddDefaultPolicy(
                        builder =>
                        {
                            _ = corsSettings.AllowAllOrigins
                                ? builder.AllowAnyOrigin() : builder.WithOrigins(CorsSettings.SplitAndTrimAllowList(corsSettings.AllowedOrigins));

                            _ = corsSettings.AllowAllMethods
                                ? builder.AllowAnyMethod() : builder.WithMethods(CorsSettings.SplitAndTrimAllowList(corsSettings.AllowedMethods));

                            _ = corsSettings.AllowAllHeaders
                                ? builder.AllowAnyHeader() : builder.WithHeaders(CorsSettings.SplitAndTrimAllowList(corsSettings.AllowedHeaders));

                            _ = corsSettings.AllowCredentials ? builder.AllowCredentials() : builder.DisallowCredentials();
                        });
            }));
        }
    }
}
