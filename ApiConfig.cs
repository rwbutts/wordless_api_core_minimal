public class ApiConfig
{
    public ApiConfig() {}
    public string ApiRootUri {get; set;} = "/";

    public static ApiConfig Default = new ();
}