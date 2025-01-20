
var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

foreach( var number in array)
{
    Console.WriteLine(number);
}

var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

foreach( var number in list)
{
    Console.WriteLine(number);
}

var resultaat = DoeIets();
Console.WriteLine(resultaat.Item1);
Console.WriteLine(resultaat.Item2);

var (nummer, tekst) = DoeIetsNieuws();
Console.WriteLine(nummer);
Console.WriteLine(tekst);


var vektor = new Vector<decimal> { X = 3.0M, Y = 4.5M };
Console.WriteLine(vektor);


static Tuple<int, string> DoeIets()
{
    return new Tuple<int, string>(1, "Hallo");
}

static (int, string) DoeIetsNieuws()
{
    return (1, "Hallo");
}


public class Vector<T> where T : struct
{
    public T X { get; set; }
    public T Y { get; set; }

    public override string ToString()
    {
        return $"{{{X};{Y}}}";
    }
}