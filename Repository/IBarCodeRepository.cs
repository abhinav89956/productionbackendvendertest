using VenderTest.DTOs;
using VenderTest.Models;

namespace VenderTest.Repository
{
    public interface IBarCodeRepository
    {
     
        Task<List<VenderItemsDto>> GetItemsByVenderCode(string venderCode);

        
        Task<IEnumerable<BarCodeDto>> GetVenderBarcodes();

        Task<BarCodeDto> DeleteBarcode(int BarcodeId);
        Task<VenderItemsDto> InsertVenderItemBarcodeAsync(
            string venderCode,
            string itemCode,
            DateTime manufacturingDate,
            DateTime expiryDate, string barcodeBase64,
                        string pdfBase64, string Varcode
        );
    }
}