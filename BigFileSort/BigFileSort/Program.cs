using System;

namespace BigFileSort
{
    class Program
    {
        static void Main(string[] args)
        {
            var chunksCreator = new ChunksCreator();
            chunksCreator.SplitFile("input", 13);
        }
    }
}