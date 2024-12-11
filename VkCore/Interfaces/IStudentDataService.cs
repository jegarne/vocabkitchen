namespace VkCore.Interfaces
{
    public interface IStudentDataService
    {
        int GetInProgressWordsCount();
        int GetKnownWordsCount();
        int GetNewReadingsCount();
        int GetNewWordsCount();
    }
}