namespace VenderTest.Repository
{
    public interface IGenericRepository
    {
        Task<T> ExecuteAsync<T>(string sp, object param = null);
        Task<int> ExecuteAsync(string sp, object param = null);

        Task<T> QueryFirstOrDefaultAsync<T>(string sp, object param = null);

        Task<IEnumerable<T>> QueryAsync<T>(string sp, object param = null);
    }
}
