using Dapper;
using Microsoft.Data.SqlClient;
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

    public async Task<T> ExecuteAsync<T>(string sp, object param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<T>(
                sp,
                param,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while executing {sp}", ex);
        }
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sp, object param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<T>(
                sp,
                param,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while executing {sp}", ex);
        }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sp, object param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<T>(
                sp,
                param,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while executing {sp}", ex);
        }
    }
    public async Task<List<T>> QueryListAsync<T>(string sp, object param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var result = await connection.QueryAsync<T>(
                sp,
                param,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while executing {sp}", ex);
        }
    }

    public async Task<int> ExecuteAsync(string sp, object param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            return await connection.ExecuteAsync(
                sp,
                param,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while executing {sp}", ex);
        }
    }
}
