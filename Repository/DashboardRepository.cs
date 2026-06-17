using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IGenericRepository _repo;

        public DashboardRepository(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardVendorDto> GetVendorByUserId(int userId)
        {
            var result = await _repo.QueryFirstOrDefaultAsync<DashboardVendorDto>(
                @"SELECT * FROM ""_vender"".sp_getvenderbyuserid(@UserId)",
                new { UserId = userId }
            );

            return result;
        }
    }
}
