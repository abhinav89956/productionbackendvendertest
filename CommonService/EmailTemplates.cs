
namespace VenderTest.CommonService
{
    public static class EmailTemplates
    {

        private static readonly string LogoUrl = "https://dynarextech.com/assets/img/logo1.png";
        public static string PasswordResetTemplate(string resetLink)
        {
            return $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    padding: 20px;
                }}
                .email-container {{
                    background-color: #ffffff;
                    padding: 30px;
                    max-width: 600px;
                    margin: auto;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0,0,0,0.1);
                }}
                .logo {{
                    text-align: center;
                    margin-bottom: 20px;
                }}
                .btn {{
                    display: inline-block;
                    padding: 10px 20px;
                    margin-top: 20px;
                    color: #ffffff;
                    background-color: #007bff;
                    text-decoration: none;
                    border-radius: 5px;
                }}
                .footer {{
                    margin-top: 30px;
                    font-size: 12px;
                    color: #888888;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='logo'>
                    <img src='{LogoUrl}' alt='Company Logo' height='60'/>
                </div>
                <h2>Password Reset Request</h2>
                <p>Hello,</p>
                <p>We received a request to reset your password. Click the button below to reset it:</p>
                <p style='text-align:center;'>
                    <a href='{resetLink}' class='btn'>Reset Password</a>
                </p>
                <p>If you did not request a password reset, please ignore this email.</p>
                <div class='footer'>
                    &copy; {DateTime.Now.Year} Your Company. All rights reserved.
                </div>
            </div>
        </body>
        </html>";
        }
    }

}

