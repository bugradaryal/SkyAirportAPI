namespace Utilitys.Logging
{
    public static class LogLevelResolver
    {
        public static int FromStatusCode(int statusCode) => statusCode switch
        {
            >= 500 => 3,   // Error
            >= 400 => 2,   // Warn
            _ => 1         // Info
        };
    }
}
