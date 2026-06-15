namespace VenderTest.DTOs
{
    public class BarCodeDto
    {
        public int BarcodeId { get; set; }

        public string VenderCode { get; set; }
        public string? Message { get; set; }
        public int? Status { get; set; }

        public string ItemCode { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string VarCode { get; set; }

        public DateTime InsertedDate { get; set; }
        public string BarcodeBase64 { get; set; }
        public string PdfBase64 { get; set; }
    }
}
