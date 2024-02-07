namespace QuackService.Api.Logic
{
    public interface ITopicFinder
    {
        string[] GetTopics(string input);
        string TrimSuffix(string input);
    }
}