namespace LinkShortener.Models;

public abstract class ShortenUrlRequest
{
    public string Url { get; set; } = string.Empty;
}