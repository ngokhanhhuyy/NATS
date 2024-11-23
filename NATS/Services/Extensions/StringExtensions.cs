namespace NATS.Services.Extensions;

public static class StringExtensions
{
    public static string ToNullIfEmpty(this string value)
    {
        return string.IsNullOrWhiteSpace(value?.Trim()) ? null : value.Trim();
    }

    public static string ToWordsFirstLetterCapitalized(this string value)
    {
        string result = string.Empty;
        for (int i = 0; i < value.Length; i++) {
            if (i == 0 || value[i - 1] == ' ' || value[i - 1] == '\n') {
                result += value[i].ToString().ToUpper();
                continue;
            }
            result += value[i];
        }
        return result;
    }
    
    public static string ToNonDiacritics(this string value)
    {
        string normalizedString = value.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();
    
        foreach (Rune rune in normalizedString.EnumerateRunes())
        {
            var unicodeCategory = Rune.GetUnicodeCategory(rune);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(rune);
            }
        }
    
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}