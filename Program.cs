using BenchmarkDotNet.Running;
using OpenTKGame.Core.Tests;

namespace OpenTKGame.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-test"))
                BenchmarkRunner.Run<Tester>();

            using (Game game = new Game(800, 600, "my game"))
            {
                game.Run();
            }
        }
    }
}