using Microsoft.EntityFrameworkCore;

namespace LinkShortener.API.Services;

public class UrlShorteningService(ApplicationDbContext dbContext)
{
    public const int NumberOfCharacters = 5;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
    
    private readonly Random _random = new Random();

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharacters];

        while (true)
        {
            for (var i = 0; i < NumberOfCharacters; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length);
            
                codeChars[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChars);

            if (!await dbContext.ShortenedUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }
        }
    }
}