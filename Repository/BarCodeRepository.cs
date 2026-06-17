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

        public async Task<BarCodeDto> DeleteBarcode(int barcodeId)
        {
            try
            {
                // SP_Barcode_Delete(p_BarcodeId)
                var result = await _repo.QueryAsync<BarCodeDto>(
                    "_vender.SP_Barcode_Delete",
                    new { BarcodeId = barcodeId }
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
                // SP_GetItemsByVenderCode(p_VenderCode)
                var result = await _repo.QueryAsync<VenderItemsDto>(
                    "_vender.SP_GetItemsByVenderCode",
                    new { VenderCode = venderCode }
                );

                return result.ToList();
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
                // SP_GetVenderItemBarcodes() — no parameters
                var result = await _repo.QueryAsync<BarCodeDto>(
                    "_vender.SP_GetVenderItemBarcodes"
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                return new List<BarCodeDto>
                {
                    new BarCodeDto { Status = 0, Message = ex.Message }
                };
            }
        }

        public async Task<VenderItemsDto> InsertVenderItemBarcodeAsync(
            string venderCode,
            string itemCode,
            DateTime manufacturingDate,
            DateTime expiryDate,
            string barcodeBase64,
            string pdfBase64,
            string varcode)
        {
            try
            {
                // SP_InsertVenderItemBarcode(p_VenderCode, p_ItemCode, p_VarCode,
                //                            p_ManufacturingDate, p_ExpiryDate,
                //                            p_BarcodeBase64, p_PdfBase64)
                var result = await _repo.QueryAsync<VenderItemsDto>(
                    "_vender.SP_InsertVenderItemBarcode",
                    new
                    {
                        VenderCode = venderCode,
                        ItemCode = itemCode,
                        VarCode = varcode,
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
