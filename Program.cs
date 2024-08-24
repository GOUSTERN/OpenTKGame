using System.Linq;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "my game"))
            {
                game.Run();
            }
        }
    }
}