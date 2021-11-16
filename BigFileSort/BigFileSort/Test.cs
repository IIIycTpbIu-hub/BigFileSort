using System.Threading;
using BenchmarkDotNet.Attributes;

namespace BigFileSort
{
    public class Test
    {
        [Benchmark]
        [Arguments(100, 10)]
        public void Method(int n, int sleep)
        {
            for (int i = 0; i < n; i++)
            {
                Thread.Sleep(sleep);
            }
        }
    }
}