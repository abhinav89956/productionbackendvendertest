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
                var result = await _repo.QueryAsync<BarCodeDto>(
                    @"SELECT * FROM ""_vender"".""sp_barcode_delete""(@p_barcodeid)",
                    new { p_barcodeid = barcodeId }
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
                    @"SELECT * FROM ""_vender"".""sp_getitemsbyvendercode""(@p_vendercode)",
                    new { p_vendercode = venderCode }
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
                var result = await _repo.QueryAsync<BarCodeDto>(
                    @"SELECT * FROM ""_vender"".""sp_getvenderitembarcodes""()"
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
                var result = await _repo.QueryAsync<VenderItemsDto>(
                    @"SELECT * FROM ""_vender"".""sp_insertvenderitembarcode""(
                    @p_vendercode,
                    @p_itemcode,
                    @p_varcode,
                    @p_manufacturingdate,
                    @p_expirydate,
                    @p_barcodebase64,
                    @p_pdfbase64)",
                    new
                    {
                        p_vendercode = venderCode,
                        p_itemcode = itemCode,
                        p_varcode = varcode,
                        p_manufacturingdate = DateOnly.FromDateTime(manufacturingDate),
                        p_expirydate = DateOnly.FromDateTime(expiryDate),
                        p_barcodebase64 = barcodeBase64,
                        p_pdfbase64 = pdfBase64
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