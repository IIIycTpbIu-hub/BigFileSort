using System;
using System.IO;
using System.Text;

namespace BigFileSort
{
    public class ChunksCreator
    {
        public void SplitFile(string filePath, int chunkSize)
        {
            var fileLenght = new FileInfo(filePath).Length;
            int chunkFilesCount = 1;
            
            if (fileLenght > chunkSize)
            {
                chunkFilesCount = (int)Math.Ceiling((decimal)fileLenght / chunkSize);
            }

            using (var inputStream = File.OpenRead(filePath))
            {
                var bufferSize = 1024;
                using (var inputStreamReader = new StreamReader(inputStream, Encoding.UTF8, true, bufferSize))
                {
                    int createdFilesCount = 0;
                    string line;
                    int readBytes = 0;

                    while ((line = inputStreamReader.ReadLine()) != null)
                    {
                        var bytes = Encoding.UTF8.GetBytes(line + '\n');
                        var chunkFileName = filePath + "_" + createdFilesCount;
                        
                        using (var outputStream = File.OpenWrite(chunkFileName))
                        {
                            outputStream.Seek(readBytes, SeekOrigin.Begin);
                            outputStream.Write(bytes);
                            readBytes += bytes.Length;
                        }
                        
                        if (readBytes > chunkSize)
                        {
                            createdFilesCount++;
                            readBytes = 0;
                        }
                    }
                }
            }
        }
        public static void SplitFile(string inputFile, int chunkSize, string path)
        {
            const int BUFFER_SIZE = 20 * 1024;
            byte[] buffer = new byte[BUFFER_SIZE];

            using (Stream input = File.OpenRead(inputFile))
            {
                int index = 0;
                while (input.Position < input.Length)
                {
                    using (Stream output = File.Create(path + "\\" + index))
                    {
                        int remaining = chunkSize, bytesRead;
                        while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                            Math.Min(remaining, BUFFER_SIZE))) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                            remaining -= bytesRead;
                        }
                    }
                    index++;
                }
            }
        }
    }
}