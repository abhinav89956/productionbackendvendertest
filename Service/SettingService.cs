using VenderTest.Models;
using VenderTest.Repository;
using VenderTest.Service;
using static Dapper.SqlMapper;

namespace VenderTest.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<Setting> GetSettings()
        {
            try
            {
                return await _settingRepository.GetSettings();
            }
            catch (Exception ex)
            {
               
                return new Setting();
            }
        }

        public async Task<Setting> UpdateSettings(Setting model)
        {
            try
            {
                return await _settingRepository.UpdateSettings(model);
            }
            catch (Exception ex)
            {
                return new Setting
                {
                    Status = 0,
                    Message = ex.Message
                };
            }
        }
    }
}