namespace WordlessApi 
{
    public class ApiConfig
    {
        public ApiConfig() {}
        public string ApiRootUri {get; set;} = "/";

        public readonly static ApiConfig Default = new ();
    }
}