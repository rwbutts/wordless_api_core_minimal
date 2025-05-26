namespace WordlessApi.Cors
{
    public class CorsSettings
    {
        public static readonly string[] WILDCARD = new string[] { "*" };
        public static readonly string[] EMPTYARRAY = Array.Empty<string>();

        public string[] AllowedOrigins { get; set; } = EMPTYARRAY;
        public string[] AllowedMethods { get; set; } = EMPTYARRAY;
        public string[] AllowedHeaders { get; set; } = EMPTYARRAY;

        // Per specification, AllowCredentials must be false if AllowedOrigins = ["*"]
        public bool AllowCredentials { get; set; } = false;

        public CorsSettings()
        {
        }

        public static CorsSettings AllowAllPolicy()
        {
            return new CorsSettings();
        }
    }
}