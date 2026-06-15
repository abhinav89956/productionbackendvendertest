namespace VenderTest.DTOs
{
    public class BarcodeCreateDto
    {
        public string VenderCode { get; set; }
        public string ItemCode { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
