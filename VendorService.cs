using VenderTest.DTOs;
using VenderTest.Models;
using VenderTest.Repository;
using YourProject.Models;

namespace VenderTest.Service
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<List<Vendor>> GetAllVenders(
       string? searchVenderCode,
       int pageNumber,
       int pageSize)
        {
            try
            {
                return await _vendorRepository.GetAllVenders(
                    searchVenderCode,
                    pageNumber,
                    pageSize
                );
            }
            catch (Exception)
            {
                return new List<Vendor>();
            }
        }
        public async Task<ApiResponseDto> AddVendor(Vendor vendor)
        {
            try
            {
                if (vendor == null)
                {
                    return new ApiResponseDto
                    {
                        Status = 0,
                        Message = "Vendor data is null"
                    };
                }

                return await _vendorRepository.AddVendor(vendor);
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Service Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto> UpdateVendor(Vendor vendor)
        {
            try
            {
                if (vendor == null || vendor.VenderId <= 0)
                {
                    return new ApiResponseDto
                    {
                        Status = 0,
                        Message = "Invalid vendor data"
                    };
                }

                return await _vendorRepository.UpdateVendor(vendor);
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Service Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto> DeleteVender(Vendor vendor)
        {
            try
            {
                if (vendor == null || vendor.VenderId <= 0)
                {
                    return new ApiResponseDto
                    {
                        Status = 0,
                        Message = "Invalid vendor ID"
                    };
                }

                return await _vendorRepository.DeleteVender(vendor);
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Service Error: {ex.Message}"
                };
            }
        }

        public async Task<VenderAsignDto> AsignItems(string ItemCode, string VenderCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ItemCode) || string.IsNullOrWhiteSpace(VenderCode))
                {
                    return new VenderAsignDto
                    {
                        Status = 0,
                        Message = "Invalid ItemCode or VendorCode"
                    };
                }

                // Pass correct parameters
                return await _vendorRepository.AsignItems(ItemCode, VenderCode);
            }
            catch (Exception ex)
            {
                return new VenderAsignDto
                {
                    Status = 0,
                    Message = $"Service Error: {ex.Message}"
                };
            }
        }

        public async Task<VenderAsignDto> UnAsignItems(string ItemCode, string VenderCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ItemCode) || string.IsNullOrWhiteSpace(VenderCode))
                {
                    return new VenderAsignDto
                    {
                        Status = 0,
                        Message = "Invalid ItemCode or VendorCode"
                    };
                }

               
                return await _vendorRepository.UnAsignItems(ItemCode, VenderCode);
            }
            catch (Exception ex)
            {
                return new VenderAsignDto
                {
                    Status = 0,
                    Message = $"Service Error: {ex.Message}"
                };
            }
        }
    }
}
