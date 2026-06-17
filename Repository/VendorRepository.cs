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
                    "_vender.SP_GetAllVenders",
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
                // SP_AddVenderWithLotStrict(p_VenderCode, p_CodeDescription, p_Email)
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    "_vender.SP_AddVenderWithLotStrict",
                    new
                    {
                        VenderCode = vendor.VenderCode,
                        CodeDescription = vendor.CodeDescription,
                        Email = vendor.Email
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
                // SP_UpdateVender(p_VenderId, p_VenderCode, p_CodeDescription, p_Email, p_IsActive)
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    "_vender.SP_UpdateVender",
                    new
                    {
                        VenderId = vendor.VenderId,
                        VenderCode = vendor.VenderCode,
                        CodeDescription = vendor.CodeDescription,
                        Email = vendor.Email,
                        IsActive = vendor.IsActive
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
                // SP_DeleteVender(p_VenderId)
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    "_vender.SP_DeleteVender",
                    new { VenderId = vendor.VenderId }
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
                // SP_VenderItems_AssignUnassign(p_VenderCode, p_ItemCode, p_Action)
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<VenderAsignDto>(
                    "_vender.SP_VenderItems_AssignUnassign",
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

        public async Task<VenderAsignDto> UnAsignItems(string itemCode, string venderCode)
        {
            try
            {
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<VenderAsignDto>(
                    "_vender.SP_VenderItems_AssignUnassign",
                    new
                    {
                        VenderCode = venderCode,
                        ItemCode = itemCode,
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
