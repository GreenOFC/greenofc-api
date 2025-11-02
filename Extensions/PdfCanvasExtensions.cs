using iText.Kernel.Pdf.Canvas;

namespace _24hplusdotnetcore.Extensions
{
    public static class PdfCanvasExtensions
    {
        public static PdfCanvas ShowTextIf(this PdfCanvas pdfCanvas, bool condition, string text)
        {
            return condition ? pdfCanvas.ShowText(text) : pdfCanvas;
        }
    }
}
