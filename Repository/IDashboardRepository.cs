using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardVendorDto> GetVendorByUserId(int userId);
    }
}
