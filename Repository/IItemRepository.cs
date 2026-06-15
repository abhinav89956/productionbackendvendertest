using VenderTest.DTOs;
using VenderTest.Models;

namespace VenderTest.Repository
{
    public interface IItemRepository
    {
        Task<ApiResponseDto> AddItem(Item item);
        Task<ApiResponseDto> UpdateItem(Item item);
        Task<ApiResponseDto> DeleteItem(int itemId);

        Task<List<ItemDto>> GetAllItems(string? searchItemCode, int pageNumber, int pageSize);
    }

}
