using Dapper;
using Npgsql;
using System.Data;
using System.Reflection;
using VenderTest.Data;
using VenderTest.Repository;

public class GenericRepository : IGenericRepository
{
    private readonly DapperDbContext _context;

    public GenericRepository(DapperDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Builds correct PostgreSQL function call with NAMED parameters.
    ///
    /// PostgreSQL functions have params named p_Email, p_Password etc.
    /// C# passes @Email, @Password.
    /// So we generate:  SELECT * FROM _vender."SP_UserLogin"(p_Email => @Email, p_Password => @Password)
    ///
    /// This works for ALL functions regardless of param order.
    /// </summary>
    private static string BuildCall(string sp, object? param)
    {
        // Strip SQL Server brackets if any: [_vender].[SP_Name] -> _vender.SP_Name
        sp = sp.Replace("[", "").Replace("]", "");

        var parts = sp.Split('.');
        var schema = parts.Length > 1 ? parts[0] : "public";
        var funcName = parts.Length > 1 ? parts[1] : parts[0];

        // Always quote function name — PostgreSQL is case-sensitive
        var quotedFunc = $"{schema}.\"{funcName}\"";

        if (param == null)
            return $"SELECT * FROM {quotedFunc}()";

        // Handle Dapper DynamicParameters
        if (param is DynamicParameters dp)
        {
            var names = dp.ParameterNames.ToList();
            if (names.Count == 0)
                return $"SELECT * FROM {quotedFunc}()";

            // Named syntax: p_ParamName => @ParamName
            var args = string.Join(", ", names.Select(n => $"p_{n} => @{n}"));
            return $"SELECT * FROM {quotedFunc}({args})";
        }

        // Handle anonymous objects / regular classes
        var props = param.GetType()
                         .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (props.Length == 0)
            return $"SELECT * FROM {quotedFunc}()";

        // Named syntax: p_Email => @Email, p_Password => @Password
        var paramList = string.Join(", ", props.Select(p => $"p_{p.Name} => @{p.Name}"));
        return $"SELECT * FROM {quotedFunc}({paramList})";
    }

    // ---------------------------------------------------------------
    // ExecuteAsync<T> — calls function, returns first row as T
    // ---------------------------------------------------------------
    public async Task<T> ExecuteAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildCall(sp, param);
            return await connection.QueryFirstOrDefaultAsync<T>(
                sql, param, commandType: CommandType.Text);
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error in {sp}: {ex.Message}", ex);
        }
    }

    // ---------------------------------------------------------------
    // QueryFirstOrDefaultAsync<T> — returns single row
    // ---------------------------------------------------------------
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildCall(sp, param);
            return await connection.QueryFirstOrDefaultAsync<T>(
                sql, param, commandType: CommandType.Text);
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error in {sp}: {ex.Message}", ex);
        }
    }

    // ---------------------------------------------------------------
    // QueryAsync<T> — returns multiple rows
    // ---------------------------------------------------------------
    public async Task<IEnumerable<T>> QueryAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildCall(sp, param);
            return await connection.QueryAsync<T>(
                sql, param, commandType: CommandType.Text);
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error in {sp}: {ex.Message}", ex);
        }
    }

    // ---------------------------------------------------------------
    // QueryListAsync<T> — returns List<T>
    // ---------------------------------------------------------------
    public async Task<List<T>> QueryListAsync<T>(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildCall(sp, param);
            var result = await connection.QueryAsync<T>(
                sql, param, commandType: CommandType.Text);
            return result.ToList();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error in {sp}: {ex.Message}", ex);
        }
    }

    // ---------------------------------------------------------------
    // ExecuteAsync — for VOID functions, returns rows affected
    // ---------------------------------------------------------------
    public async Task<int> ExecuteAsync(string sp, object? param = null)
    {
        try
        {
            using var connection = _context.CreateConnection();
            var sql = BuildCall(sp, param);
            return await connection.ExecuteAsync(
                sql, param, commandType: CommandType.Text);
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"Database error in {sp}: {ex.Message}", ex);
        }
    }
}
