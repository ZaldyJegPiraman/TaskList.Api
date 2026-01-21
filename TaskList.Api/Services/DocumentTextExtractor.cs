//using System.Text;
//using UglyToad.PdfPig;

//namespace TaskList.Api.Services
//{
//    public class DocumentTextExtractor
//    {

//        public async Task<string> ExtractAsync(IFormFile file)
//        {
//            var extension = Path.GetExtension(file.FileName).ToLower();

//            if (extension == ".txt")
//            {
//                using var reader = new StreamReader(file.OpenReadStream());
//                return await reader.ReadToEndAsync();
//            }

//            if (extension == ".pdf")
//            {
//                using var pdf = PdfDocument.Open(file.OpenReadStream());
//                var text = new StringBuilder();

//                foreach (var page in pdf.GetPages())
//                    text.AppendLine(page.Text);

//                return text.ToString();
//            }

//            throw new NotSupportedException("Only .txt and .pdf supported for see demo");
//        }
//    }
//}



using System.Text;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TaskList.Api.Services
{
    public class DocumentTextExtractor
    {
        public async Task<string> ExtractAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();

            // ✅ TXT
            if (extension == ".txt")
            {
                using var reader = new StreamReader(file.OpenReadStream());
                return await reader.ReadToEndAsync();
            }

            // ✅ PDF
            if (extension == ".pdf")
            {
                using var pdf = PdfDocument.Open(file.OpenReadStream());
                var text = new StringBuilder();

                foreach (var page in pdf.GetPages())
                {
                    text.AppendLine(page.Text);
                }

                return text.ToString();
            }

            // ✅ DOCX
            if (extension == ".docx")
            {
                using var stream = file.OpenReadStream();
                using var wordDoc = WordprocessingDocument.Open(stream, false);

                var body = wordDoc.MainDocumentPart?.Document.Body;
                if (body == null)
                    return string.Empty;

                var text = new StringBuilder();

                foreach (var paragraph in body.Elements<Paragraph>())
                {
                    text.AppendLine(paragraph.InnerText);
                }

                return text.ToString();
            }

            throw new NotSupportedException("Only .txt, .pdf, and .docx are supported for this demo");
        }
    }
}

