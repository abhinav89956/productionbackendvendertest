namespace VenderTest.Repository
{
    public interface IGenericRepository
    {
        Task<T> ExecuteAsync<T>(string sqlOrFunction, object param = null);

        Task<int> ExecuteAsync(string sql, object param = null);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);
    }
}