namespace WordlessApi.Config
{
    public class ApiSettings
    {

        public ApiSettings() { }
        public string? ApiRootUri { get; set; } = null;
        public string? PathBase { get; set; } = null;
        public string? WebRootPath { get; set; } = null;
        public string? ContentRootPath { get; set; } = null;

        public readonly static ApiSettings Default = new();
    }

    static class ApiSettingsExtensions
    {
        const string DEFAULT_API_CONFIG_PATH = "WordlessApi";

        public static WebApplicationBuilder CreateCustomApiBuilder(string[] args, string configPath = DEFAULT_API_CONFIG_PATH)
        {
            var tempBuilder = WebApplication.CreateBuilder(args);
            var apiSettings = tempBuilder.Configuration
                                .GetSection(configPath)
                                .Get<ApiSettings>() ?? ApiSettings.Default;

            WebApplicationOptions opts = new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = apiSettings.ContentRootPath,
                WebRootPath = apiSettings.WebRootPath
            };

            var builder = WebApplication.CreateBuilder(opts);
            builder.Services.AddSingleton(apiSettings);

            return builder;
        }

        public static void ConfigureApiPathBase(this WebApplication app)
        {
            var apiSettings = app.Services.GetRequiredService<ApiSettings>();

            if (!String.IsNullOrEmpty(apiSettings.PathBase))
            {
                app.UsePathBase(apiSettings.PathBase);
            }
        }

        public static RouteGroupBuilder CreateApiRouteGroup(this WebApplication app)
        {
            var apiSettings = app.Services.GetRequiredService<ApiSettings>();
            var apiRootUri = !String.IsNullOrEmpty(apiSettings.ApiRootUri) ? apiSettings.ApiRootUri : "/";

            return app.MapGroup(apiRootUri);
        }

   }

}