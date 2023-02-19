namespace FileProcessingAPI.Interfaces
{
    public interface IFileRepository
    {
        string[] GetInvoicesForProcessing(string path);
        byte[] GetFileBytes(string file);
    }
}