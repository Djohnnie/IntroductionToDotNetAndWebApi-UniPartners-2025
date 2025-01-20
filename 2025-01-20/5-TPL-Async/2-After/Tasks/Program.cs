
var task1 = new Task<string>(() => SchrijfNaarConsole("Taak 1"));
var task2 = new Task<string>(() => SchrijfNaarConsole("Taak 2"));

task1.Start();
task2.Start();

SchrijfNaarConsole("Buiten taak 1 en taak 2");

while (!task1.IsCompleted && !task2.IsCompleted)
{
    Thread.Sleep(100);
}

Console.WriteLine("Taak 1 is voltooid: " + task1.Result);
Console.WriteLine("Taak 2 is voltooid: " + task2.Result);

Console.ReadKey();


static string SchrijfNaarConsole(string tekst)
{
    var volledigeTekst = $"Geschreven vanuit thread {Thread.CurrentThread.ManagedThreadId}: {tekst}";
    Console.WriteLine(volledigeTekst);
    return volledigeTekst;
}