using VenderTest.DTOs;

namespace VenderTest.Services
{
    public interface IBarCodeService
    {
        Task<List<VenderItemsDto>> GetItemsByVenderCode(string venderCode);

        Task<IEnumerable<BarCodeDto>> GetVenderBarcodes();
        Task<BarCodeDto> DeleteBarcode(int BarcodeId);

        Task<VenderItemsDto> InsertVenderItemBarcodeAsync(
            string venderCode,
            string itemCode,
            DateTime manufacturingDate,
            DateTime expiryDate, string Varcode
        );
    }
}