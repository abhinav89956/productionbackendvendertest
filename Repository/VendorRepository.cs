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
                    @"SELECT * FROM ""_vender"".sp_getallvenders(
                        @SearchVenderCode,
                        @PageNumber,
                        @PageSize)",
                    new
                    {
                        SearchVenderCode = searchVenderCode,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    });

                return venders.ToList();
            }
            catch
            {
                return new List<Vendor>();
            }
        }

        public async Task<ApiResponseDto> AddVendor(Vendor vendor)
        {
            try
            {
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".sp_addvenderwithlotstrict(
                        @VenderCode,
                        @CodeDescription,
                        @Email)",
                    new
                    {
                        VenderCode = vendor.VenderCode,
                        CodeDescription = vendor.CodeDescription,
                        Email = vendor.Email
                    });

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
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".sp_updatevender(
                        @VenderId,
                        @VenderCode,
                        @CodeDescription,
                        @Email,
                        @IsActive)",
                    new
                    {
                        VenderId = vendor.VenderId,
                        VenderCode = vendor.VenderCode,
                        CodeDescription = vendor.CodeDescription,
                        Email = vendor.Email,
                        IsActive = vendor.IsActive
                    });

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
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".sp_deletevender(
                        @VenderId)",
                    new
                    {
                        VenderId = vendor.VenderId
                    });

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
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<VenderAsignDto>(
                    @"SELECT * FROM ""_vender"".sp_venderitems_assignunassign(
                        @VenderCode,
                        @ItemCode,
                        @Action)",
                    new
                    {
                        VenderCode = venderCode,
                        ItemCode = itemCode,
                        Action = "ASSIGN"
                    });

                return spResult ?? new VenderAsignDto
                {
                    Status = 0,
                    Message = "No response from database"
                };
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
                    @"SELECT * FROM ""_vender"".sp_venderitems_assignunassign(
                        @VenderCode,
                        @ItemCode,
                        @Action)",
                    new
                    {
                        VenderCode = venderCode,
                        ItemCode = itemCode,
                        Action = "UNASSIGN"
                    });

                return spResult ?? new VenderAsignDto
                {
                    Status = 0,
                    Message = "No response from database"
                };
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