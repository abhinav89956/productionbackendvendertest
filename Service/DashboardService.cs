using VenderTest.DTOs;
using VenderTest.Repository;

namespace VenderTest.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<DashboardVendorDto> GetVendorDetails(int userId)
        {
            if (userId <= 0)
                throw new Exception("Invalid User Id");

            var result = await _repository.GetVendorByUserId(userId);

            return result;
        }
    }
}