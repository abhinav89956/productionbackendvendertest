using VenderTest.DTOs;
using VenderTest.Models;
using VenderTest.Repository;
using VenderTest.Service;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ApiResponseDto> AddItem(Item item)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(item.ItemCode))
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Item Code is required."
                };
            }

            return await _itemRepository.AddItem(item);
        }
        catch (Exception ex)
        {
            return new ApiResponseDto
            {
                Status = 0,
                Message = $"Service error while adding item: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponseDto> UpdateItem(Item item)
    {
        try
        {
            if (item.ItemId <= 0)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Invalid Item Id."
                };
            }

            if (string.IsNullOrWhiteSpace(item.ItemCode))
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Item Code is required."
                };
            }

            return await _itemRepository.UpdateItem(item);
        }
        catch (Exception ex)
        {
            return new ApiResponseDto
            {
                Status = 0,
                Message = $"Service error while updating item: {ex.Message}"
            };
        }
    }

 
    public async Task<ApiResponseDto> DeleteItem(int itemId)
    {
        try
        {
            if (itemId <= 0)
            {
                return new ApiResponseDto
                {
                    Status = 0,
                    Message = "Invalid Item Id."
                };
            }

            return await _itemRepository.DeleteItem(itemId);
        }
        catch (Exception ex)
        {
            return new ApiResponseDto
            {
                Status = 0,
                Message = $"Service error while deleting item: {ex.Message}"
            };
        }
    }
    public async Task<List<ItemDto>> GetAllItems(string? searchItemCode, int pageNumber, int pageSize)
    {
        try
        {
            return await _itemRepository.GetAllItems(searchItemCode, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            return new List<ItemDto>();
        }
    }

}
