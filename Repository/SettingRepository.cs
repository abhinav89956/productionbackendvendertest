using VenderTest.Models;

namespace VenderTest.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private readonly IGenericRepository _genericRepository;

        public SettingRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Setting> GetSettings()
        {
            // ✅ FIX - SELECT syntax
            var result = await _genericRepository.QueryFirstOrDefaultAsync<Setting>(
                @"SELECT * FROM ""_vender"".""sp_settings_get""()"
            );

            return result;
        }

        public async Task<Setting> UpdateSettings(Setting model)
        {
            // ✅ FIX - SELECT syntax + p_ prefix
            var result = await _genericRepository.QueryFirstOrDefaultAsync<Setting>(
                @"SELECT * FROM ""_vender"".""sp_settings_update""(
                @p_id,
                @p_minexpirymonths,
                @p_manufacturingdays,
                @p_expirytokenhrs)",
                new
                {
                    p_id = model.Id,
                    p_minexpirymonths = model.MinExpiryMonths,
                    p_manufacturingdays = model.ManufacturingDays,
                    p_expirytokenhrs = model.ExpiryTokenHrs
                }
            );

            return result;
        }
    }
}