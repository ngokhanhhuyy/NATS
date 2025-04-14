namespace NATS.Helpers;

public class CssUniqueIdHelper
{
    private readonly List<string> _generatedIds = new List<string>();
    private readonly Random _random = new Random();
    private static readonly string _characters;

    static CssUniqueIdHelper()
    {
        _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    }

    public string GenerateId()
    {
        int idLength = _random.Next(10, 15);
        string id;
        do
        {
            char[] characters = new char[idLength];
            for (int index = 0; index < idLength; index++)
            {
                characters[index] = _characters[_random.Next(0, idLength)];
            }

            id = new string(characters);
        }
        while (!_generatedIds.Contains(id));

        return id;
    }
}
