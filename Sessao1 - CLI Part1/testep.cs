
namespace Sessao1
{
    class Program
    {
        static void Main(string[] args)
        {
            Ponto p1 = new Ponto(1, 5);
            Ponto p2 = new Ponto(5, 6);
            System.Console.Out.WriteLine(p1.ToString());
            System.Console.Out.WriteLine(p2.ToString());
            System.Console.Out.WriteLine(p1.Distance(p2));

            Ponto p3 = new Ponto(1, 4);
            Ponto p4 = new Ponto(1, 4);
            System.Console.Out.WriteLine(p3.CompareTo(p4));
        }
    }
}
