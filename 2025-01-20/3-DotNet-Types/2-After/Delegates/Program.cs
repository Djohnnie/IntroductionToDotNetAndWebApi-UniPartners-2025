

var som = Hulp.Som(5, 4);
var somDelegate = new Hulp.SomDelegate(Hulp.Som);

som = somDelegate(81, 19);

som = DubbeleSom(1, 2, Hulp.Som);
som = DubbeleSom(1, 2, (a, b) => a + b);
som = NieuweDubbeleSom(1, 2, (a, b) => a + b);



static int DubbeleSom(int a, int b, Hulp.SomDelegate somDelegate)
{
    return somDelegate(a, b) + somDelegate(a, b);
}

static int NieuweDubbeleSom(int a, int b, Func<int, int, int> somDelegate)
{
    return somDelegate(a, b) + somDelegate(a, b);
}


class Hulp
{
    public delegate int SomDelegate(int a, int b);

    public static int Som(int a, int b)
    {
        return a + b;
    }
}