
Console.Write("Geef een stukje tekst in kleine letters: ");
string invoer = Console.ReadLine();

string resultaat = invoer.GeefElkWoordHoofdletters();
Console.WriteLine($"Elk woord met hoofletter: {resultaat}");

Console.ReadKey();

static class StringExtensions
{
    public static string GeefElkWoordHoofdletters(this string invoer)
    {
        string[] woorden = invoer.Split(' ');
        string resultaat = "";

        foreach (string woord in woorden)
        {
            string eersteLetter = woord.Substring(0, 1).ToUpper();
            string restVanHetWoord = woord.Substring(1);
            resultaat += eersteLetter + restVanHetWoord + " ";
        }

        return resultaat;
    }
}