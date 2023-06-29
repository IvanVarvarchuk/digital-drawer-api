
using System;
using System.Drawing;
using System.Linq;
using Accord.Imaging.Filters;
using Accord.Math.Geometry;
using Accord.Statistics.Kernels;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models.Geometry;
using LinqKit;
using OpenCvSharp;
using Line = DigitalDrawer.Application.Common.Models.Geometry.Line;
using Point = System.Drawing.Point;

namespace DigitalDrawer.Infrastructure.ImageProcessing;

public class ImageProcessingService : IImageProcessingService
{
    private Line FormatLine(LineSegmentPoint lineSegment, int imageWidth, int imageHeight)
    {
        Point startPoint = new Point()
        {
            X = lineSegment.P1.X * 100 / imageWidth,
            Y = lineSegment.P1.Y * 100 / imageHeight,
        };
        Point endPoint = new Point()
        {
            X = lineSegment.P2.X * 100 / imageWidth,
            Y = lineSegment.P2.Y * 100 / imageHeight
        };
        return new Line(startPoint, endPoint);  
    }

    public IEnumerable<Line> ExtractGeometry(Stream imageStream)
    {
        LineSegmentPoint[] lines = new LineSegmentPoint[] { };
        using Mat image = Mat.FromStream(imageStream, ImreadModes.Color);
        int imageWidth = image.Width;
        int imageHeight = image.Height;
        using (Mat gray = new Mat())
        using (Mat binary = new Mat())
        using (Mat edge = new Mat())
        {
            // Перетворення зображення у відтінки сірого
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            // Бінаризація зображення
            Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            var parameters = binary.SelectParameters();
            Cv2.Canny(binary, edge, parameters.LowThreshold, parameters.HighThreshold);
            // Знаходження контурів
            lines = Cv2.HoughLinesP(edge, 1, Math.PI / 180, 25);
        }
        return lines.Select(l => FormatLine(l, imageWidth, imageHeight));       
    }
}
