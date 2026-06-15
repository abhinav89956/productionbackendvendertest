using VenderTest.DTOs;
using VenderTest.Models;

namespace VenderTest.Service
{
    public interface IItemService
    {
        Task<ApiResponseDto> AddItem(Item item);
        Task<ApiResponseDto> UpdateItem(Item item);
        Task<ApiResponseDto> DeleteItem(int itemId);
        Task<List<ItemDto>> GetAllItems(string? searchItemCode, int pageNumber, int pageSize);

    }

}
