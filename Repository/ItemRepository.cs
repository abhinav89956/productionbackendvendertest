using VenderTest.Data;
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
            var spResult= await _genericRepository.ExecuteAsync<ApiResponseDto>(
                "[_vender].[SP_Items_Add]",
                new
                {
                    item.ItemCode,
                    item.ItemDescription,
                    item.UPC
                }
                
            );
            return spResult;
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
            var spResult = await _genericRepository.ExecuteAsync<ApiResponseDto>(
                "[_vender].[SP_Items_Update]",
                new
                {
                    item.ItemId,
                    item.ItemCode,
                    item.ItemDescription,
                    item.UPC
                }
            );
            return spResult;
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
            var spResult = await _genericRepository.ExecuteAsync<ApiResponseDto>(
                 "[_vender].[SP_Items_Delete]",
                new
                {
                    ItemId = itemId
                }
            );
            return spResult;
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
                "[_vender].[SP_GetAllItems]",
                new
                {
                    SearchItemCode = searchItemCode,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            );

            return items.ToList(); 
        }
        catch (Exception ex)
        {
           
            return new List<ItemDto>(); 
        }
    }

}
