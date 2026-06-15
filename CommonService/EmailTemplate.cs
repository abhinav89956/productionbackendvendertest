using VenderTest.Models;
using YourProject.Models;

namespace VenderTest.CommonService
{
    public class EmailTemplate
    {
        public static string GetWelcomeEmailHtml(Vendor vendor, string link)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>Welcome</title>
</head>

<body style='margin:0;padding:0;background-color:#f2f5f9;font-family:Segoe UI,Arial,sans-serif;'>

<table width='100%' cellpadding='0' cellspacing='0' style='background:#f2f5f9;padding:30px 0;'>

<tr>
<td align='center'>

<table width='600' cellpadding='0' cellspacing='0' style='background:#ffffff;border-radius:10px;overflow:hidden;box-shadow:0 6px 20px rgba(0,0,0,0.08);'>

<!-- Header -->
<tr>
<td style='background:#1a73e8;padding:25px;text-align:center;color:white;'>
<h1 style='margin:0;font-size:26px;'>Vendor Portal</h1>
<p style='margin:5px 0 0 0;font-size:14px;'>Welcome to our platform</p>
</td>
</tr>

<!-- Content -->
<tr>
<td style='padding:35px;'>

<p style='font-size:16px;color:#333;margin-bottom:15px;'>
Hi <strong>{vendor.VenderCode}</strong>,
</p>

<p style='font-size:15px;color:#555;line-height:1.6;'>
We are excited to have you join the <b>Vendor Portal</b>.  
Your account has been successfully created.
</p>

<p style='font-size:15px;color:#555;line-height:1.6;'>
Click the button below to access your account and get started.
</p>

<!-- Button -->
<table width='100%' style='margin:30px 0;text-align:center;'>
<tr>
<td align='center'>
<a href='{link}' 
style='background:#1a73e8;
color:#ffffff;
padding:14px 32px;
font-size:15px;
font-weight:600;
text-decoration:none;
border-radius:6px;
display:inline-block;'>
Access Your Account
</a>
</td>
</tr>
</table>

<p style='font-size:14px;color:#777;line-height:1.6;'>
If you did not sign up for this account, please ignore this email.
</p>

</td>
</tr>

<!-- Footer -->
<tr>
<td style='background:#f8f9fb;padding:20px;text-align:center;font-size:13px;color:#999;'>

<p style='margin:0;'>© 2026 Vendor Portal. All rights reserved.</p>

<p style='margin:6px 0 0 0;font-size:12px;'>
This is an automated email. Please do not reply.
</p>

</td>
</tr>

</table>

</td>
</tr>

</table>

</body>
</html>";
        }
    }
}