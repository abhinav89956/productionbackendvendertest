using VenderTest.DTOs;
using YourProject.Models;

namespace VenderTest.Service
{
    public interface IVendorService
    {
        Task<List<Vendor>> GetAllVenders(
            string? searchVenderCode,
            int pageNumber,
            int pageSize
        );


        Task<ApiResponseDto> AddVendor(Vendor vendor);

        Task<ApiResponseDto> UpdateVendor(Vendor vendor);

        Task<ApiResponseDto> DeleteVender(Vendor vendor);

        Task<VenderAsignDto> AsignItems(string ItemCode, string VenderCode);

        Task<VenderAsignDto> UnAsignItems(string ItemCode, string VenderCode);
    }
}