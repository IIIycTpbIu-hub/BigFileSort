using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BigFileSort.Sort.Impl;

namespace BigFileSort
{
    class Program
    {
        private const int CHUNK_SIZE = 67108864; //91751 кб (90мб)
        
        static void Main(string[] args)
        {
            var chunkSize = 3;
            var bufferSize = 65536;
            var processor = new ChunksProcessor(chunkSize, bufferSize);

            var chunkFiles = processor.SplitFile("test");
            var fileSorter = new FileSorter();
            
            var tasks = new List<Task>(chunkFiles.Count);

            foreach (var file in chunkFiles)
            {
                tasks.Add(new Task(() => fileSorter.Sort(file)));
            }

            Parallel.ForEach(
                tasks,
                new ParallelOptions() {MaxDegreeOfParallelism = chunkFiles.Count},
                (task) => task.Start());

            Task.WaitAll(tasks.ToArray());

            var sortedLines = processor.Sort(chunkFiles);

            foreach (var file in chunkFiles)
            {
                File.Delete(file);
            }

            foreach (var line in sortedLines)
            {
                Console.WriteLine(line);
            }
        }
    }
}