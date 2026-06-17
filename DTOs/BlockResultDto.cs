namespace VenderTest.DTOs
{
    /// <summary>
    /// Maps the result of SP_BlockUser which returns ("Message", "IsBlocked")
    /// </summary>
    public class BlockResultDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
    }

    /// <summary>
    /// Maps the result of SP_SendOtp which returns ("Status", "Message", "OtpCode")
    /// </summary>
    public class OtpResultDto
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }
}
