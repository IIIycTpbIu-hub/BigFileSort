using System.IO;
using System.Text;
using BigFileSort.FileIO;

namespace BigFileSort
{
    public class ChunksCreator
    {
        public void SplitFile(string filePath, int chunkSize)
        {
            int createdFilesCount = 0;
            int wroteBytes = 0;
            var file = new FileReader(filePath, 1024);
            
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
    }
}