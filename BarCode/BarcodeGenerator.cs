using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace VenderTest.BarCode
{
    public class BarcodeGenerator
    {
        private readonly HttpClient _http;

        public BarcodeGenerator(HttpClient http)
        {
            _http = http;
        }

   public string GenerateZplQr(string VarCode)
{
    return $@"
^XA
^FO50,50
^BQN,2,10
^FDLA,{VarCode}^FS
^XZ";
}


        public async Task<byte[]> GenerateBarcodeImageAsync(string varCode)
        {
            var zpl = GenerateZplQr(varCode);
            var content = new StringContent(zpl, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _http.PostAsync("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public byte[] GenerateBarcodePdf(byte[] barcodeBlob, string VarCode)
        {
            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            var img = Image.GetInstance(barcodeBlob);
            doc.Add(new Paragraph($"VarCode: {VarCode}"));
            doc.Add(img);

            doc.Close();
            return ms.ToArray();
        }
    }
}
