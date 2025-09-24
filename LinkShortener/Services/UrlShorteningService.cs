using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Services;

public class UrlShorteningService
{
    public const int NumberOfCharacters = 5;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
    
    private readonly Random _random = new Random();
    private readonly ApplicationDbContext _dbContext;

    public UrlShorteningService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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

            if (!await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }
        }
    }
}