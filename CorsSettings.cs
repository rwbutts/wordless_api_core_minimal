namespace WordlessApi.Config
{
    public class CorsSettings
    {
        public static readonly string WILDCARD = "*";
        public string AllowedOrigins { get; set; } = WILDCARD;
        public string AllowedMethods { get; set; } = WILDCARD;
        public string AllowedHeaders { get; set; } = WILDCARD;

        public bool AllowAllOrigins { get => IsWildCard(AllowedOrigins);}
        public bool AllowAllHeaders { get => IsWildCard(AllowedHeaders);}
        public bool AllowAllMethods { get => IsWildCard(AllowedMethods);}

        // Per specification, AllowCredentials must be false if AllowedOrigins = ["*"]
        public bool AllowCredentials { get; set; } = false;

        public CorsSettings()
        {
        }

        public void Validate()
        {
            //Console.WriteLine($"{this.ToJsonString()} O {SplitAndTrimValue(AllowedOrigins).ToJsonString()}, H {SplitAndTrimValue(AllowedHeaders).ToJsonString()},  M {SplitAndTrimValue(AllowedMethods).ToJsonString()}");
            if (AllowAllOrigins && AllowCredentials)
            {
                throw new InvalidOperationException("CORS configuration is invalid: AllowCredentials cannot be used with AllowedOrigins '*' (the default).");
            }   
        }

        public static string[] SplitAndTrimAllowList(string s)
        {
            return s.Split(", ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool IsWildCard(string SettingsValue)
        {
            return SettingsValue.Trim() == WILDCARD;
        }

        public static CorsSettings CreateAllowAllPolicy()
        {
            return new CorsSettings();
        }
    }
}