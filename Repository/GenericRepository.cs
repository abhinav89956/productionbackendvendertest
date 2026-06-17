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

    // KEY DIFFERENCE vs SQL Server:
    // PostgreSQL functions are called with CommandType.Text using SELECT * FROM schema.function(...)
    // SQL Server used CommandType.StoredProcedure with "[schema].[SP_Name]"
    // Helper: converts "schema.FunctionName" or "[schema].[FunctionName]" -> "SELECT * FROM schema."FunctionName"(...)"

    private static string BuildFunctionCall(string sp, object? param)
    {
        // Strip SQL Server brackets: [_vender].[SP_Name] -> _vender.SP_Name
        sp = sp.Replace("[", "").Replace("]", "");

        if (param == null)
            return $"SELECT * FROM {sp}()";

        // Build named parameter list:  SELECT * FROM _vender.SP_Name(p_Email => @Email, ...)
        var props = param.GetType().GetProperties();
        if (props.Length == 0)
            return $"SELECT * FROM {sp}()";

        // Split schema.FunctionName
        var parts = sp.Split('.');
        var schema = parts.Length > 1 ? parts[0] : "public";
        var funcName = parts.Length > 1 ? parts[1] : parts[0];

        // PostgreSQL function parameter prefix is p_ (matches our SQL script)
        // We pass them as positional params using Dapper anonymous objects
        var paramNames = string.Join(", ", props.Select(p => $"@{p.Name}"));
        return $"SELECT * FROM {schema}.\"{funcName}\"({paramNames})";
    }

    private static string BuildVoidFunctionCall(string sp, object? param)
    {
        sp = sp.Replace("[", "").Replace("]", "");
        var parts = sp.Split('.');
        var schema = parts.Length > 1 ? parts[0] : "public";
        var funcName = parts.Length > 1 ? parts[1] : parts[0];

        if (param == null)
            return $"SELECT {schema}.\"{funcName}\"()";

        var props = param.GetType().GetProperties();
        if (props.Length == 0)
            return $"SELECT {schema}.\"{funcName}\"()";

        var paramNames = string.Join(", ", props.Select(p => $"@{p.Name}"));
        return $"SELECT {schema}.\"{funcName}\"({paramNames})";
    }

    /// <summary>
    /// Calls a PostgreSQL function and returns the first row mapped to T.
    /// Replaces the old StoredProcedure ExecuteAsync pattern.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildFunctionCall(sp, param);

            return await connection.QueryFirstOrDefaultAsync<T>(
                sql,
                param,
                commandType: CommandType.Text
            );
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error while executing {sp}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Calls a PostgreSQL function that returns a single row.
    /// </summary>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildFunctionCall(sp, param);

            return await connection.QueryFirstOrDefaultAsync<T>(
                sql,
                param,
                commandType: CommandType.Text
            );
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error while executing {sp}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Calls a PostgreSQL function that returns multiple rows.
    /// </summary>
    public async Task<IEnumerable<T>> QueryAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildFunctionCall(sp, param);

            return await connection.QueryAsync<T>(
                sql,
                param,
                commandType: CommandType.Text
            );
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error while executing {sp}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Calls a PostgreSQL function that returns multiple rows as a List.
    /// </summary>
    public async Task<List<T>> QueryListAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildFunctionCall(sp, param);

            var result = await connection.QueryAsync<T>(
                sql,
                param,
                commandType: CommandType.Text
            );

            return result.ToList();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error while executing {sp}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Calls a PostgreSQL VOID function (no return value, e.g. UpdateLastSeen).
    /// Returns rows affected.
    /// </summary>
    public async Task<int> ExecuteAsync(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildVoidFunctionCall(sp, param);

            return await connection.ExecuteAsync(
                sql,
                param,
                commandType: CommandType.Text
            );
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error while executing {sp}: {ex.Message}", ex);
        }
    }
}
