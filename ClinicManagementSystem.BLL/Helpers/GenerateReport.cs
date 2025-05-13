using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using Newtonsoft.Json.Linq;


namespace ClinicManagementSystem.BLL.Helpers
{
    public class GenerateReport : IGenerateReport
    {
        private readonly HttpClient _httpClient;

        public GenerateReport(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> ExportApiResponseToPdfAsync(string apiUrl , string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var root = JsonDocument.Parse(responseString).RootElement;

            using (var stream = new MemoryStream())
            {
                var doc = new PdfSharp.Pdf.PdfDocument();
                var page = doc.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 12, XFontStyleEx.Regular);

                int y = 50;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in root.EnumerateObject())
                    {
                        gfx.DrawString($"{property.Name}: {property.Value}", font, XBrushes.Black, new XRect(20, y, page.Width, page.Height));
                        y += 20;
                    }
                }
                else if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in root.EnumerateArray())
                    {
                        foreach (var prop in item.EnumerateObject())
                        {
                            gfx.DrawString($"{prop.Name}: {prop.Value}", font, XBrushes.Black, new XRect(20, y, page.Width - 40, 20), XStringFormats.TopLeft);
                            y += 20;

                            if (y > page.Height - 40)
                            {
                                page = doc.AddPage();
                                gfx = XGraphics.FromPdfPage(page);
                                y = 50;
                            }
                        }

                        y += 20; // Extra spacing between items
                    }
                }

                doc.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
