using VenderTest.DTOs;
using VenderTest.Models;
using VenderTest.Repository;

public class ItemRepository : IItemRepository
{
    private readonly IGenericRepository _genericRepository;

    public ItemRepository(IGenericRepository genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<ApiResponseDto> AddItem(Item item)
    {
        try
        {
            var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                @"SELECT * FROM ""_vender"".sp_items_add(
                    @ItemCode,
                    @ItemDescription,
                    @UPC)",
                new
                {
                    item.ItemCode,
                    item.ItemDescription,
                    item.UPC
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
                Message = $"Error while adding item: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponseDto> UpdateItem(Item item)
    {
        try
        {
            var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                @"SELECT * FROM ""_vender"".sp_items_update(
                    @ItemId,
                    @ItemCode,
                    @ItemDescription,
                    @UPC)",
                new
                {
                    item.ItemId,
                    item.ItemCode,
                    item.ItemDescription,
                    item.UPC
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
                Message = $"Error while updating item: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponseDto> DeleteItem(int itemId)
    {
        try
        {
            var spResult = await _genericRepository.QueryFirstOrDefaultAsync<ApiResponseDto>(
                @"SELECT * FROM ""_vender"".sp_items_delete(
                    @ItemId)",
                new
                {
                    ItemId = itemId
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
                Message = $"Error while deleting item: {ex.Message}"
            };
        }
    }

    public async Task<List<ItemDto>> GetAllItems(string? searchItemCode, int pageNumber, int pageSize)
    {
        try
        {
            var items = await _genericRepository.QueryAsync<ItemDto>(
                @"SELECT * FROM ""_vender"".sp_getallitems(
                    @SearchItemCode,
                    @PageNumber,
                    @PageSize)",
                new
                {
                    SearchItemCode = searchItemCode,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

            return items.ToList();
        }
        catch
        {
            return new List<ItemDto>();
        }
    }
}