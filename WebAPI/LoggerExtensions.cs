namespace WebAPI;

public static class LoggerExtensions
{
    #region push-property-extension-method
    public static IDisposable? PushProperty(this ILogger logger, string propertyName, object propertyValue)
    {
        return logger.BeginScope(new Dictionary<string, object?>
        {
            { propertyName, propertyValue }
        });
    }
    #endregion
}
