using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BigFileSort.FileIO;

namespace BigFileSort.Sort.Impl
{
    public class ChunksProcessor
    {
        private readonly int _chunkSize;
        private readonly int _bufferSize;

        public ChunksProcessor(int chunkSize, int bufferSize)
        {
            _chunkSize = chunkSize;
            _bufferSize = bufferSize;
        }
        
        public List<string> SplitFile(string filePath)
        {
            int createdFilesCount = 0;
            int wroteBytes = 0;
            
            var chunks = new List<string>();
            string chunkName = filePath + "_" + createdFilesCount;
            chunks.Add(chunkName);
            
            var file = new FileReader(filePath);
            var streamWriter = new StreamWriter(chunkName, false, Encoding.UTF8, _bufferSize);

            try
            {
                foreach (var line in file)
                {
                    streamWriter.WriteLine(line);
                    wroteBytes += line.Length;

                    if (wroteBytes > _chunkSize)
                    {
                        streamWriter.Dispose();
                        createdFilesCount++;
                        wroteBytes = 0;
                        chunkName = filePath + "_" + createdFilesCount;
                        chunks.Add(chunkName);
                        streamWriter = new StreamWriter(chunkName, false, Encoding.UTF8, _bufferSize);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                streamWriter.Dispose();
            }

            return chunks;
        }
        
        public List<string> Sort(List<string> chunkNames)
        {
            var files = new List<IEnumerator<string>>(chunkNames.Count);
            var lines = new List<string>(chunkNames.Count);
            
            foreach (var file in chunkNames)
            {
                files.Add(new FileReader(file).GetEnumerator());
                lines.Add(default);
            }

            var result = new List<string>();

            string inserted = default;

            while (true)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i] == default)
                    {
                        if (files[i].MoveNext())
                        {
                            lines[i] = files[i].Current;
                        }
                        else
                        {
                            files.RemoveAt(i);
                            lines.RemoveAt(i);
                        }
                    }
                }

                if (lines.Count == 0)
                {
                    break;
                }
                
                string max = lines[0];
                int maxIndex = 0;
                for (int i = 0; i < lines.Count; i++)
                {
                    var current = lines[i];
                    var position = String.Compare(current, max);
                    
                    if (position <= 0)
                    {
                        max = current;
                        maxIndex = i;
                    }
                }

                if (max != null)
                {
                    if (inserted != max)
                    {
                        result.Add(max);
                        inserted = max;   
                    }
                    
                    lines[maxIndex] = default;
                }
            }

            return result;
        }
    }
}