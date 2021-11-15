using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigFileSort.FileIO
{
    //Probably same as File.ReadLines
    public class FileReader : IEnumerable<string>
    {
        private readonly string _fileName;
        private readonly int _bufferSize;

        public FileReader(string fileName, int bufferSize)
        {
            _fileName = fileName;
            _bufferSize = bufferSize;
        }

        public IEnumerator<string> GetEnumerator()
        {
            using (var inputStream = File.OpenRead(_fileName))
            {
                using (var inputStreamReader = new StreamReader(inputStream, Encoding.UTF8, true, _bufferSize))
                {
                    string line;
                    while ((line = inputStreamReader.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}