namespace Business.Redis
{
    public interface IRedisServices
    {
        Task SetAsync(string key, string value);
        Task<string?> GetAsync(string key);
    }
}
