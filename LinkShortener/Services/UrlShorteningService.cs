namespace LinkShortener.Services;

public class UrlShorteningService
{
    private const int NumberOfCharacters = 5;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
    
    private readonly Random _random = new Random();

    public string GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharacters];

        for (var i = 0; i < NumberOfCharacters; i++)
        {
            var randomIndex = _random.Next(Alphabet.Length - 1);
            
            codeChars[i] = Alphabet[randomIndex];
        }
        
        return new string(codeChars);
    }
}