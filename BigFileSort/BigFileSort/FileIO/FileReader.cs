using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BigFileSort.FileIO
{
    //Probably same as File.ReadLines
    public class FileReader : IEnumerable<string>
    {
        private readonly string _fileName;

        public FileReader(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerator<string> GetEnumerator()
        {
            using (StreamReader sr = File.OpenText(_fileName))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    yield return s;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}