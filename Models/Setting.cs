namespace VenderTest.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public int? MinExpiryMonths { get; set; }
        public int ?MaxExpiryMonths { get; set; }
        public int? ManufacturingDays { get; set; }
        public int? ExpiryTokenHrs { get; set; }
        public string? Message { get; set; }
        public int? Status { get; set; }
    }
}
