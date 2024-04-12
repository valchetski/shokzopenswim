namespace Shokz;

public static class UriUtil
{
    public static UriType GetUriType(string uri)
    {
        return uri switch
        {
            string when Uri.TryCreate(uri, UriKind.Absolute, out Uri? uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) 
                => UriType.Http,
            string when Path.Exists(uri) => UriType.Local,
            _ => UriType.Unknown
        };
    }
}
