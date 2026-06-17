using Dapper;
using Npgsql;
using System.Data;
using VenderTest.Data;
using VenderTest.Repository;

public class GenericRepository : IGenericRepository
{
    private readonly DapperDbContext _context;

    public GenericRepository(DapperDbContext context)
    {
        _context = context;
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
    {
        using var conn = _context.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<T>(
            sql,
            param,
            commandType: CommandType.Text
        );
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
    {
        using var conn = _context.CreateConnection();

        return await conn.QueryAsync<T>(
            sql,
            param,
            commandType: CommandType.Text
        );
    }

    public async Task<int> ExecuteAsync(string sql, object param = null)
    {
        using var conn = _context.CreateConnection();

        return await conn.ExecuteAsync(
            sql,
            param,
            commandType: CommandType.Text
        );
    }

    public async Task<T> ExecuteAsync<T>(string sqlOrFunction, object param = null)
    {
        using var conn = _context.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<T>(
            sqlOrFunction,
            param,
            commandType: CommandType.Text
        );
    }
}