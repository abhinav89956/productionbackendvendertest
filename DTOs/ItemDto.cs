namespace VenderTest.DTOs
{
    public class ItemDto
    {
        public int Status { get; set; }      
        public string? Message { get; set; }   
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public long UPC { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? TotalCount { get; set; }     
    }

}
