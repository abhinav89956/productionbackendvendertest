namespace VenderTest.DTOs
{
    public class VenderItemsDto
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public int VenderId { get; set; }
        public string VenderCode { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int? BarcodeId { get; set; }
        public string? VarCode { get; set; }
        public string? barcodeBase64 { get; set; }
        public string? pdfBase64 { get; set; }
    }

}
