namespace UrlShortener.Core;

public static class Base62EncodingScenarios
{
    private const string AlphaNumeric = 
        "0123456789" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ"+
        "abcdefghijklmnopqrstuvwxyz";
    public static string EncodeToBase62(this long number)
    {
        if(number == 0) return AlphaNumeric[0].ToString();

        var result = new Stack<char>();
        while (number > 0)
        {
            result.Push(AlphaNumeric[(int)(number % 62)]);
            number /= 62;
        }

        return new string(result.ToArray());
    }
}