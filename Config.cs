public class Config
{
    public Config() {}
    public string ApiRootUri {get; set;} = "/";

    public static Config Default = new ();
}