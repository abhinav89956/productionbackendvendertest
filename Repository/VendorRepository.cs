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
                    @"SELECT * FROM ""_vender"".""sp_getallvenders""(
                    @p_searchvendercode,
                    @p_pagenumber,
                    @p_pagesize)",
                    new
                    {
                        p_searchvendercode = string.IsNullOrWhiteSpace(searchVenderCode)
                            ? null
                            : searchVenderCode,
                        p_pagenumber = pageNumber,
                        p_pagesize = pageSize
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
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".""sp_addvenderwithlotstrict""(
                    @p_vendercode,
                    @p_codedescription,
                    @p_email)",
                    new
                    {
                        p_vendercode = vendor.VenderCode,
                        p_codedescription = vendor.CodeDescription,
                        p_email = vendor.Email
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
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".""sp_updatevender""(
                    @p_venderid,
                    @p_vendercode,
                    @p_codedescription,
                    @p_email,
                    @p_isactive)",
                    new
                    {
                        p_venderid = vendor.VenderId,
                        p_vendercode = vendor.VenderCode,
                        p_codedescription = vendor.CodeDescription,
                        p_email = vendor.Email,
                        p_isactive = vendor.IsActive
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
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                    @"SELECT * FROM ""_vender"".""sp_deletevender""(
                    @p_venderid)",
                    new
                    {
                        p_venderid = vendor.VenderId
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
                var spResult = await _genericRepository.QueryFirstOrDefaultAsync<VenderAsignDto>(
                    @"SELECT * FROM ""_vender"".""sp_venderitems_assignunassign""(
                    @p_vendercode,
                    @p_itemcode,
                    @p_action)",
                    new
                    {
                        p_vendercode = venderCode,
                        p_itemcode = itemCode,
                        p_action = "ASSIGN"
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
                    @"SELECT * FROM ""_vender"".""sp_venderitems_assignunassign""(
                    @p_vendercode,
                    @p_itemcode,
                    @p_action)",
                    new
                    {
                        p_vendercode = venderCode,
                        p_itemcode = itemCode,
                        p_action = "UNASSIGN"
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