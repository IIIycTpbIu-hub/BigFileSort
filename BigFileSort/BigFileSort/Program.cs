using System;
using System.Diagnostics;
using BigFileSort.FileIO;

namespace BigFileSort
{
    class Program
    {
        static void Main(string[] args)
        {
            var chunkSize = 67108864;//67108864 - 91751 кб (90мб)
            var bufferSize = 65536;
            var chunksCreator = new ChunksCreator();
            var sw = new Stopwatch();
            sw.Start();
            chunksCreator.SplitFile("input", chunkSize, bufferSize);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            //BenchmarkRunner.Run<ChunksCreator>();
        }
    }
}