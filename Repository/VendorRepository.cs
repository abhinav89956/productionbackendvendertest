using VenderTest.DTOs;
using VenderTest.Models;
using YourProject.Models;

namespace VenderTest.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly IGenericRepository _genericRepository;

        public VendorRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<Vendor>> GetAllVenders(
      string? searchVenderCode,
      int pageNumber,
      int pageSize)
        {
            try
            {
                var venders = await _genericRepository.QueryAsync<Vendor>(
                    "[_vender].[SP_GetAllVenders]",
                    new
                    {
                        SearchVenderCode = searchVenderCode,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }
                );

                return venders.ToList();
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
                var spResult = await _genericRepository.ExecuteAsync<ApiResponseDto>(
                    "[_vender].[SP_AddVenderWithLotStrict]",
                    new
                    {
                        VenderCode = vendor.VenderCode,
                        CodeDescription = vendor.CodeDescription,
                        Email = vendor.Email,
                    }
                );

                return spResult ?? new ApiResponseDto
                {
                    Status = 0,
                    Message = "No response from database"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Error while adding vendor: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto> UpdateVendor(Vendor vendor)
        {
            try
            {
                var spResult = await _genericRepository.ExecuteAsync<ApiResponseDto>(
                    "[_vender].[SP_UpdateVender]",
                    new
                    {
                        VenderId = vendor.VenderId,
                        VenderCode = vendor.VenderCode,
                        CodeDescription = vendor.CodeDescription,
                        Email = vendor.Email,
                      IsActive=vendor.IsActive
                    }
                );

                return spResult ?? new ApiResponseDto
                {
                    Status = 0,
                    Message = "No response from database"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Error while updating vendor: {ex.Message}"
                };
            }
        }


        public async Task<ApiResponseDto> DeleteVender(Vendor vendor)
        {
            try
            {
                var spResult = await _genericRepository.ExecuteAsync<ApiResponseDto>(
                    "[_vender].[SP_DeleteVender]",
                    new
                    {
                        VenderId = vendor.VenderId
                    }
                );

                return spResult ?? new ApiResponseDto
                {
                    Status = 0,
                    Message = "No response from database"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Error while deleting vendor: {ex.Message}"
                };
            }
        }


        public async Task<VenderAsignDto> AsignItems(string itemCode, string venderCode)
        {
            try
            {
                var spResult = await _genericRepository.ExecuteAsync<VenderAsignDto>(
                    "[_vender].[SP_VenderItems_AssignUnassign]",
                    new
                    {
                        VenderCode = venderCode,
                        ItemCode = itemCode,
                        Action = "ASSIGN" 
                    }
                );

                return spResult!;
            }
            catch (Exception ex)
            {
                return new VenderAsignDto
                {
                    Status = 0,
                    Message = $"Error while assigning items: {ex.Message}"
                };
            }
        }

        public async Task<VenderAsignDto> UnAsignItems(string ItemCode, string VenderCode)
        {
            try
            {
                var spResult = await _genericRepository.ExecuteAsync<VenderAsignDto>(
                    "[_vender].[SP_VenderItems_AssignUnassign]",
                    new
                    {
                        ItemCode = ItemCode,
                        VenderCode = VenderCode,
                        Action = "UNASSIGN"
                    }
                );

                return spResult!;
            }
            catch (Exception ex)
            {
                return new VenderAsignDto
                {
                    Status = 0,
                    Message = $"Error while unassigning items: {ex.Message}"
                };
            }
        }
    }
}
