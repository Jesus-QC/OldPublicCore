using Core.Bot;

try
{
    new Bot(int.Parse(args[0])).Run(args[1]).GetAwaiter().GetResult();
}
catch (Exception e)
{
    Console.WriteLine(e);
}