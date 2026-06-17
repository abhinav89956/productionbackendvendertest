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
            // SP_Settings_Get() — no parameters
            var result = await _genericRepository.QueryFirstOrDefaultAsync<Setting>(
                "_vender.SP_Settings_Get"
            );

            return result;
        }

        public async Task<Setting> UpdateSettings(Setting model)
        {
            // SP_Settings_Update(p_Id, p_MinExpiryMonths, p_ManufacturingDays, p_ExpiryTokenHrs)
            var result = await _genericRepository.QueryFirstOrDefaultAsync<Setting>(
                "_vender.SP_Settings_Update",
                new
                {
                    Id = model.Id,
                    MinExpiryMonths = model.MinExpiryMonths,
                    ManufacturingDays = model.ManufacturingDays,
                    ExpiryTokenHrs = model.ExpiryTokenHrs
                }
            );

            return result;
        }
    }
}
