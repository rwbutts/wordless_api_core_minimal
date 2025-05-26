using Newtonsoft.Json;

namespace WordlessApi
{
    public static class Utils
    {
        public static string ToJsonString(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }

}
