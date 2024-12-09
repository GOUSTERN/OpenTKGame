using BenchmarkDotNet.Attributes;

namespace OpenTKGame.Core.Tests
{
    public class Tester
    {
        [Benchmark]
        public int Test()
        {
            return sizeof(int);
        }
    }
}