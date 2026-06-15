using VenderTest.Models;
using static Dapper.SqlMapper;

namespace VenderTest.Service
{
    public interface ISettingService
    {
        Task<Setting> GetSettings();

        Task<Setting> UpdateSettings(Setting model);
    }
}
