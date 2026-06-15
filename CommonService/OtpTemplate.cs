namespace VenderTest.CommonService
{
    public class OtpTemplate
    {
        private static readonly string LogoUrl = "https://dynarextech.com/assets/img/logo1.png";

        public static string GetOtpTemplate(string otp)
        {
            return $@"
            <div style='max-width:600px;
                        margin:auto;
                        font-family:Arial,Helvetica,sans-serif;
                        border:1px solid #e0e0e0;
                        padding:20px;
                        border-radius:10px;
                        background-color:#ffffff;'>

                <!-- Logo -->
                <div style='text-align:center;padding:10px'>
                    <img src='{LogoUrl}' alt='Company Logo' style='max-width:150px;margin-bottom:10px;' />
                    <h2 style='color:#2E86C1;margin-bottom:5px'>
                        Password Reset OTP
                    </h2>
                    <p style='color:#555'>
                        Use the OTP below to reset your password
                    </p>
                </div>

                <!-- OTP Code -->
                <div style='text-align:center;
                            background:#f4f6f7;
                            padding:20px;
                            border-radius:8px;
                            margin:20px 0;'>

                    <h1 style='letter-spacing:5px;
                               color:#2E86C1;
                               margin:0'>
                        {otp}
                    </h1>
                </div>

                <!-- Important Notes -->
                <div style='color:#555;font-size:14px'>
                    <p><strong>Important:</strong></p>
                    <ul>
                        <li>This OTP is valid for <strong>5 minutes</strong></li>
                        <li>Do not share this OTP with anyone</li>
                    </ul>
                </div>

                <!-- Footer -->
                <div style='margin-top:30px;
                            font-size:12px;
                            color:#999;
                            text-align:center'>
                    <p>
                        If you did not request a password reset, please ignore this email.
                    </p>
                </div>
            </div>";
        }
    }
}
