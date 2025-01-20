
var task1 = new Task(() => SchrijfNaarConsole("Taak 1"));
var task2 = new Task(() => SchrijfNaarConsole("Taak 2"));

task1.Start();
task2.Start();

SchrijfNaarConsole("Buiten taak 1 en taak 2");

Console.ReadKey();


static void SchrijfNaarConsole(string tekst)
{
    Console.WriteLine($"Geschreven vanuit thread {Thread.CurrentThread.ManagedThreadId}: {tekst}");
}