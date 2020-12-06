namespace OutOfLensWebsite.Models
{
    public class Messages
    {
        public static string Success(string reason = null)
        {
            return reason == null 
                ?  "{{\"success\":true\"}}"
                : $"{{\"success\":true, \"reason\": \"{reason}\"}}";
        }

        public static string Failure(string reason = null)
        {
            return reason == null 
            ?  "{\"success\":false\"}"
            : $"{{\"success\":false, \"reason\": \"{reason}\"}}";
        }
    }
}