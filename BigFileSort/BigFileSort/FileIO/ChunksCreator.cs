using System;
using System.IO;
using System.Text;

namespace BigFileSort.FileIO
{
    public class ChunksCreator
    {
        public void SplitFile(string filePath, int chunkSize, int buffer)
        {
            int createdFilesCount = 0;
            int wroteBytes = 0;
            var file = new FileReader(filePath);
            
            var streamWriter = new StreamWriter(filePath + "_" + createdFilesCount, false, Encoding.UTF8, buffer);

            try
            {
                foreach (var line in file)
                {
                    var bytes = Encoding.UTF8.GetBytes(line);
                    streamWriter.WriteLine(line);
                    wroteBytes += bytes.Length;

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
    }
}