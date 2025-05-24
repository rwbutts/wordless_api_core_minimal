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
}