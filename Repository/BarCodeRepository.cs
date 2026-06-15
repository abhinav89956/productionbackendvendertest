using System.Linq;
using VenderTest.BarCode;
using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public class BarCodeRepository : IBarCodeRepository
    {
        private readonly IGenericRepository _repo;
        
        public BarCodeRepository(IGenericRepository repo)
        {
            _repo = repo;
             
        }

        public async Task<BarCodeDto> DeleteBarcode(int BarcodeId)
        {
            try
            {
                var result= await _repo.QueryAsync<BarCodeDto>(
                    "[_vender].[SP_Barcode_Delete]",
                    new { BarcodeId = BarcodeId }
                );
                return result.FirstOrDefault() ?? new BarCodeDto
                {
                    Status = 0,
                    Message = "Barcode not deleted"
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<VenderItemsDto>> GetItemsByVenderCode(string venderCode)
        {
            try
            {
                var result = await _repo.QueryAsync<VenderItemsDto>(
                    "[_vender].[SP_GetItemsByVenderCode]",
                    new { VenderCode = venderCode }
                );

                var data = result.ToList();

                return data;
            }
            catch (Exception ex)
            {
                return new List<VenderItemsDto>
        {
            new VenderItemsDto { Status = 0, Message = ex.Message }
        };
            }
        }

        public async Task<IEnumerable<BarCodeDto>> GetVenderBarcodes()
        {
            try
            {
                var result = await _repo.QueryAsync<BarCodeDto>(
                    "[_vender].[SP_GetVenderItemBarcodes]"
                );

                var data = result.ToList();

                return data;
            }
            catch (Exception ex)
            {
                return new List<BarCodeDto>
        {
            new BarCodeDto
            {
                Status = 0,
                Message = ex.Message
            }
        };
            }
        }

        public async Task<VenderItemsDto> InsertVenderItemBarcodeAsync(
         string venderCode, string itemCode, DateTime manufacturingDate, DateTime expiryDate, string barcodeBase64, string pdfBase64, string Varcode
            )
        {
            try
            {
                var result = await _repo.QueryAsync<VenderItemsDto>(
                    "[_vender].[sp_InsertVenderItemBarcode]",
                    new
                    {
                        VarCode= Varcode,
                        VenderCode = venderCode,
                        ItemCode = itemCode,
                        ManufacturingDate = manufacturingDate,
                        ExpiryDate = expiryDate,
                        BarcodeBase64 = barcodeBase64,
                        PdfBase64 = pdfBase64
                    }
                );

                return result.FirstOrDefault() ?? new VenderItemsDto
                {
                    Status = 0,
                    Message = "Barcode not generated"
                };
            }
            catch (Exception ex)
            {
                return new VenderItemsDto
                {
                    Status = 0,
                    Message = ex.Message
                };
            }
        }

        
    }
}
