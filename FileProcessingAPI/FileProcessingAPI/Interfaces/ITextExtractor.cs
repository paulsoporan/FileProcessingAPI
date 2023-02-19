namespace FileProcessingAPI.Interfaces
{
    public interface IITextExtractor
    {
        string GetText(byte[] bytes);
    }
}