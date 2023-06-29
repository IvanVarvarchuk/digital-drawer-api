using DigitalDrawer.Application.Common.Helpers;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models.Geometry;

namespace DigitalDrawer.Infrastructure.DrawingConvertion
{
    public class SvgConvertionService : IFileConversionService
    {
        public byte[] Convert(IEnumerable<Line> lines)
        {
            SvgWriter svgWriter = new SvgWriter();
            return svgWriter.WriteLinesToSvg(lines);
        }
        public class SvgWriter
        {
            public byte[] WriteLinesToSvg(IEnumerable<Line> lines)
            {
                using var memoryStream = new MemoryStream();

                using var writer = new StreamWriter(memoryStream);

                writer.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");

                foreach (var line in lines)
                {
                    writer.WriteLine($"<line x1=\"{line.Start.X}\" y1=\"{line.Start.Y}\" x2=\"{line.End.X}\" y2=\"{line.End.Y}\" stroke=\"black\" />");
                }

                writer.WriteLine("</svg>");
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.ReadAllBytes();
            }
        }
    }
}
