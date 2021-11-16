using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BigFileSort.FileIO;

namespace BigFileSort.Benchmark
{
    [MemoryDiagnoser(true)]
    public class ChunksCreatorBenchmark
    {
        [Benchmark]
        //[Arguments("input", 5, 1048576)]
        //[Arguments("input", 5, 1024)]
        [Arguments("input", 200000, 200000)]
        public void SplitFile(string filePath, int chunkSize, int buffer)
        {
            var rnd = new Random();
            int createdFilesCount = 0;
            int wroteBytes = 0;
            var file = new FileReader(filePath, buffer);
            var streamWriter = new FileStream(filePath + "_" + createdFilesCount, FileMode.OpenOrCreate);

            try
            {
                foreach (var line in file)
                {
                    wroteBytes += WriteLine(line + '\n', streamWriter, wroteBytes);
                    
                    if (wroteBytes > chunkSize)
                    {
                        streamWriter.Dispose();
                        createdFilesCount++;
                        wroteBytes = 0;
                        streamWriter = new FileStream(filePath + "_" + createdFilesCount, FileMode.OpenOrCreate);
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
        }
        
        /*[Benchmark]
        //[Arguments("input", 262144, 1048576)]
        //[Arguments("input", 262144, 1024)]
        [Arguments("input", 200000, 0)]*/
        public void SplitFile2(string filePath, int chunkSize, int buffer)
        {
            int createdFilesCount = 0;
            int wroteBytes = 0;
            var file = new BigFileSort.FileIO.FileReader(filePath);
            
            foreach (var line in file)
            {
                var chunkFileName = filePath + "_" + createdFilesCount;

                wroteBytes += WriteLine(line + '\n', chunkFileName, wroteBytes);
                        
                if (wroteBytes > chunkSize)
                {
                    createdFilesCount++;
                    wroteBytes = 0;
                }
            }
        }
        
        [Benchmark]
        [Arguments("input", 200000, 0)]
        public void SplitFile3(string filePath, int chunkSize, int buffer)
        {
            int createdFilesCount = 0;
            int wroteBytes = 0;
            var file = new BigFileSort.FileIO.FileReader(filePath);
            
            var streamWriter = new FileStream(filePath + "_" + createdFilesCount, FileMode.OpenOrCreate);

            try
            {
                foreach (var line in file)
                {
                    wroteBytes += WriteLine(line + '\n', streamWriter, wroteBytes);
                    
                    if (wroteBytes > chunkSize)
                    {
                        streamWriter.Dispose();
                        createdFilesCount++;
                        wroteBytes = 0;
                        streamWriter = new FileStream(filePath + "_" + createdFilesCount, FileMode.OpenOrCreate);
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
        }

        [Benchmark]
        [Arguments("input", 200000, 200000)]
        public void SplitFile5(string filePath, int chunkSize, int buffer)
        {
            int createdFilesCount = 0;
            int wroteBytes = 0;
            var file = new BigFileSort.FileIO.FileReader(filePath);
            
            var streamWriter = new StreamWriter(filePath + "_" + createdFilesCount, false, Encoding.UTF8, buffer);

            try
            {
                foreach (var line in file)
                {
                    wroteBytes += WriteLine(line + '\n', streamWriter, wroteBytes);
                    
                    if (wroteBytes > chunkSize)
                    {
                        streamWriter.Dispose();
                        createdFilesCount++;
                        wroteBytes = 0;
                        streamWriter = new StreamWriter(filePath + "_" + createdFilesCount, false, Encoding.UTF8, buffer);
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
        }
        
        /*[Benchmark]
        [Arguments("input", 200000, 256)]*/
        public void SplitFile4(string filePath, int chunkSize, int buffer)
        {
            int createdFilesCount = 0;
            int wroteBytes = 0;
            var file = new BigFileSort.FileIO.FileReader(filePath);
            
            var streamWriter = new StreamWriter(filePath + "_" + createdFilesCount);

            try
            {
                foreach (var line in file)
                {
                    wroteBytes += WriteLine(line + '\n', streamWriter, wroteBytes);
                    
                    if (wroteBytes > chunkSize)
                    {
                        streamWriter.Dispose();
                        createdFilesCount++;
                        wroteBytes = 0;
                        streamWriter = new StreamWriter(filePath + "_" + createdFilesCount);
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
        }

        private int WriteLine(string line, FileStream stream, int offset)
        {
            var bytes = Encoding.UTF8.GetBytes(line);
            
            stream.Seek(offset, SeekOrigin.Begin);
            stream.Write(bytes);

            return bytes.Length;
        }
        
        private int WriteLine(string line, StreamWriter stream, int offset)
        {
            var bytes = Encoding.UTF8.GetBytes(line);
            
            stream.WriteLine(line);

            return bytes.Length;
        }

        private int WriteLine(string line, string fileName, int offset)
        {
            var bytes = Encoding.UTF8.GetBytes(line);
            
            using (var fileStream = File.OpenWrite(fileName))
            {
                fileStream.Seek(offset, SeekOrigin.Begin);
                fileStream.Write(bytes);
            }
            
            return bytes.Length;
        }
        private class FileReader : IEnumerable<string>
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
}