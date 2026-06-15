using VenderTest.DTOs;
using VenderTest.Models;
using static Dapper.SqlMapper;

        namespace VenderTest.Repository
    {
        public interface ISettingRepository
        {
            Task<Setting> GetSettings();

            Task<Setting> UpdateSettings(Setting model);
        }
    }


