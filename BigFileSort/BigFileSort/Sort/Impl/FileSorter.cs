using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigFileSort.FileIO;

namespace BigFileSort.Sort.Impl
{
    public class FileSorter : IFileSorter
    {
        private const int BUFFER_ZISE = 65536;

        public void Sort(string fileName)
        {
            var file = new FileReader(fileName);
            var dictionary = new Dictionary<string, string>();
            foreach (var line in file)
            {
                dictionary.TryAdd(line, line);
            }
            var uniqueLines = dictionary.Keys.ToArray();
            
            Array.Sort(uniqueLines);

            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8, BUFFER_ZISE))
            {
                foreach (var line in uniqueLines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        public async Task SortAsync(string fileName)
        {
            var file = new AsyncFileReader(fileName);
            var dictionary = new Dictionary<string, string>();

            await foreach (var line in file)
            {
                dictionary.TryAdd(line, line);
            }

            Console.WriteLine("Finish read file, length {0}", dictionary.Keys.Count);
            
            var uniqueLines = dictionary.Keys.ToArray();
            
            Array.Sort(uniqueLines);

            Console.WriteLine("Finish sorting");
            
            await using (var writer = new StreamWriter(fileName, false, Encoding.UTF8, BUFFER_ZISE))
            {
                foreach (var line in uniqueLines)
                {
                    await writer.WriteLineAsync(line);
                }
            }

            Console.WriteLine("Finish writing");
        }
    }
}