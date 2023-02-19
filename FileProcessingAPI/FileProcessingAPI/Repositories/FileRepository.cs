using FileProcessingAPI.Interfaces;

namespace FileProcessingAPI.Repositories
{
    public class FileRepository : IFileRepository
    {

        public FileRepository() { }

        public string[] GetInvoicesForProcessing(string path)
        {
            var files = Directory.GetFiles(path);

            return files;
        }

        public byte[] GetFileBytes(string fileName)
        {
            var bytes =  File.ReadAllBytes(fileName);

            return bytes;
        }
    }
}
