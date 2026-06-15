using VenderTest.DTOs;
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
            var result = await _genericRepository.QueryFirstOrDefaultAsync<Setting>(
                "[_vender].[SP_Settings_Get]"
            );
            return result;
        }
        public async Task<Setting> UpdateSettings(Setting model)
        {
            var result = await _genericRepository.QueryFirstOrDefaultAsync<Setting>(
                "[_vender].[SP_Settings_Update]",
                new
                {
                    model.Id,
                    model.MinExpiryMonths,
                    model.ManufacturingDays,  
                    model.ExpiryTokenHrs
                }
            );

            return result;
        }
    }
}