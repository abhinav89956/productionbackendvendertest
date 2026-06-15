using VenderTest.DTOs;

namespace VenderTest.Service
{
    public interface IDashboardService
    {
        Task<DashboardVendorDto> GetVendorDetails(int userId);
    }
}
