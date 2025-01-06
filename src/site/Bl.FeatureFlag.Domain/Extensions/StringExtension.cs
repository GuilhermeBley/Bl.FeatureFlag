using System.Globalization;
using System.Text;

namespace Bl.FeatureFlag.Domain.Extensions;

public static class StringExtension
{
    /// <summary>
    /// Removes accentuations from the string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>A string without accentuations.</returns>
    public static string RemoveAccents(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Normalize the string to decompose characters with accents
        var normalizedString = input.Normalize(NormalizationForm.FormD);

        var stringBuilder = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark) // Exclude accent marks
            {
                stringBuilder.Append(c);
            }
        }

        // Normalize back to Form C to compose the string
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}

