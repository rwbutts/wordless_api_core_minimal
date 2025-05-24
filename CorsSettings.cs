namespace WordlessApi.Cors
{
    public class CorsSettings
    {
        public static readonly string[] WILDCARD = new string[] { "*" };
        public static readonly string[] EMPTYARRAY = Array.Empty<string>();

        public string[]? AllowedOrigins { get; set; }
        public string[]? AllowedMethods { get; set; }
        public string[]? AllowedHeaders { get; set; }
        public bool AllowCredentials { get; set; } = false;

        public CorsSettings()
        {
        }

        public static CorsSettings AllowAllPolicy()
        {
            var cfg = new CorsSettings();
            cfg.AllowedOrigins = cfg.AllowedHeaders = cfg.AllowedMethods = WILDCARD;
            cfg.AllowCredentials = true;
            return cfg;
        }
    }
}