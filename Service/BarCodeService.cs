using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenderTest.BarCode;
using VenderTest.DTOs;
using VenderTest.Repository;

namespace VenderTest.Services
{
    public class BarCodeService : IBarCodeService
    {
        private readonly IBarCodeRepository _repository;
        private readonly BarcodeGenerator _barcodeGenerator;

        public BarCodeService(IBarCodeRepository repository, IHttpClientFactory httpFactory)
        {
            _repository = repository;
            _barcodeGenerator = new BarcodeGenerator(httpFactory.CreateClient());
        }

        public async Task<IEnumerable<BarCodeDto>> GetVenderBarcodes()
        {
            try
            {
                var result = await _repository.GetVenderBarcodes();

                if (result == null || !result.Any())
                {
                    return new List<BarCodeDto>
            {
                new BarCodeDto
                {
                    Status = 0,
                    Message = "No barcodes found"
                }
            };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<BarCodeDto>
        {
            new BarCodeDto
            {
                Status = 0,
                Message = $"Service error: {ex.Message}"
            }
        };
            }
        }
        public async Task<List<VenderItemsDto>> GetItemsByVenderCode(string venderCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(venderCode))
                {
                    return new List<VenderItemsDto>
                    {
                        new VenderItemsDto
                        {
                            Status = 0,
                            Message = "Vendor code is required"
                        }
                    };
                }

                var result = await _repository.GetItemsByVenderCode(venderCode);

              

                return result;
            }
            catch (Exception ex)
            {
                return new List<VenderItemsDto>
                {
                    new VenderItemsDto
                    {
                        Status = -1,
                        Message = $"Service error: {ex.Message}"
                    }
                };
            }
        }
        public async Task<VenderItemsDto> InsertVenderItemBarcodeAsync(
            string venderCode,
            string itemCode,
            DateTime manufacturingDate,
            DateTime expiryDate, string  Varcode)
        {
            try
            {
                
                string VarCode = $"{venderCode}-{itemCode}-{DateTime.Now:yyyyMMddHHmmss}";

               
                var barcodeBlob = await _barcodeGenerator.GenerateBarcodeImageAsync(VarCode);
                string barcodeBase64 = Convert.ToBase64String(barcodeBlob);

              
                var pdfBytes = _barcodeGenerator.GenerateBarcodePdf(barcodeBlob, VarCode);
                string pdfBase64 = Convert.ToBase64String(pdfBytes);

                
                var resultFromDb = await _repository.InsertVenderItemBarcodeAsync(
                    venderCode,
                    itemCode,
                    manufacturingDate,
                    expiryDate,
                    barcodeBase64,
                    pdfBase64, VarCode
                );

                return new VenderItemsDto
                {
                    Status = resultFromDb.Status,
                    VarCode = resultFromDb.VarCode,
                    barcodeBase64 = barcodeBase64,
                    pdfBase64 = pdfBase64,
                    Message = resultFromDb.Message
                };
            }
            catch (Exception ex)
            {
                return new VenderItemsDto
                {
                    Status = -1,
                    Message = $"Service error: {ex.Message}"
                };
            }
        }

        public async Task<BarCodeDto> DeleteBarcode(int barcodeId)
        {
            try
            {
                if (barcodeId <= 0)
                {
                    return new BarCodeDto
                    {
                        Status = 0,
                        Message = "Invalid Item Id."
                    };
                }

                return await _repository.DeleteBarcode(barcodeId);
            }
            catch (Exception ex)
            {
                return new BarCodeDto
                {
                    Status = -1,
                    Message = $"Service error: {ex.Message}"
                };
            }
        }
    }
    
    }
