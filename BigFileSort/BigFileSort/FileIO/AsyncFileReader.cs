using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BigFileSort.FileIO
{
    public class AsyncFileReader : IAsyncEnumerable<string>
    {
        private readonly string _fileName;

        public AsyncFileReader(string fileName)
        {
            _fileName = fileName;
        }

        public async IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            using (StreamReader sr = File.OpenText(_fileName))
            {
                string s = String.Empty;
                while ((s = await sr.ReadLineAsync()) != null)
                {
                    yield return s;
                }
            }
        }

        public async Task<string[]> ReadAllLinesAsync(string fileName)
        {
            var lines = new List<string>();
            using (StreamReader reader = File.OpenText(fileName))
            {
                string line = String.Empty;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }
    }
}