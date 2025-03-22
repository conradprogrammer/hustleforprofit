using System;
using System.IO;
using System.Text;
using iText.Html2pdf;

public static class clsPDFGenerator
{
    public static void GeneratePDF(string combinedHtml, string outputPath)
    {
        // Ensure output directory exists
        string directory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Create a PDF writer instance
        using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        {
            // Convert HTML to PDF
            HtmlConverter.ConvertToPdf(combinedHtml, fileStream);
        }
    }
}
