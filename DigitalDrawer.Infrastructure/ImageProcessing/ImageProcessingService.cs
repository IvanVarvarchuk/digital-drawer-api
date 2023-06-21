
using System;
using System.Drawing;
using System.Linq;
using DigitalDrawer.Application.Common.Interfaces;
using LinqKit;
using OpenCvSharp;
using Line = DigitalDrawer.Application.Common.Models.Geometry.Line;
using Point = System.Drawing.Point;

namespace DigitalDrawer.Infrastructure.ImageProcessing
{
    public class ImageProcessingService : IImageProcessingService
    {
        private (double lowThreshold, double highThreshold) CannyParameters(Mat image)
        {
            // Compute the gradient magnitude and direction using Sobel operators
            using Mat dx = new Mat();
            using Mat dy = new Mat();
            Cv2.Sobel(image, dx, MatType.CV_16S, 1, 0);
            Cv2.Sobel(image, dy, MatType.CV_16S, 0, 1);
            using Mat magnitude = new Mat();
            using Mat angle = new Mat();
            Cv2.CartToPolar(dx, dy, magnitude, angle, true);
            // Compute the histogram of gradient magnitudes
            int[] histSize = { 256 }; // Кількість бінів гістограми
            Rangef[] ranges = { new Rangef(0, 256) }; // Діапазон значень градієнтів
            int[] channels = { 0 }; // Канал для обчислення гістограми
            using Mat hist = new Mat();
            Cv2.CalcHist(new[] { magnitude }, channels, null, hist, 1, histSize, ranges);
            double[] peaks = new double[histSize[0]];
            hist.GetArray(out peaks);

            // Вибір порогів з вибірковим відсіванням границь піку гістограми
            double peakThreshold = 0.8; // Порог відсівання границь піку
            double maxPeakValue = peaks.Max();
            double highThreshold = 0;
            double lowThreshold = 0;

            for (int i = 1; i < histSize[0] - 1; i++)
            {
                double prevPeak = peaks[i - 1];
                double currentPeak = peaks[i];
                double nextPeak = peaks[i + 1];

                if (currentPeak > peakThreshold * maxPeakValue)
                {
                    if (currentPeak > prevPeak && currentPeak > nextPeak)
                    {
                        // Знаходимо значення порогів, які відповідають границям піку
                        double binWidth = ranges[0].End - ranges[0].Start;
                        double binValue = ranges[0].Start + (i * binWidth / histSize[0]);
                        if (lowThreshold == 0)
                            lowThreshold = binValue;
                        else
                            highThreshold = binValue;
                    }
                }
            }

            // Переконайтеся, що значення порогів встановлені
            if (highThreshold == 0)
                highThreshold = 0.9 * maxPeakValue;

            if (lowThreshold == 0)
                lowThreshold = 0.1 * maxPeakValue;
            return (lowThreshold, highThreshold);
        }
        private Line FormatLine(LineSegmentPolar polarLine)
        {
            double rho = polarLine.Rho;
            double theta = polarLine.Theta;

            double a = Math.Cos(theta);
            double b = Math.Sin(theta);
            double x0 = a * rho;
            double y0 = b * rho;

            Point startPoint = new Point((int)(x0 + 1000 * (-b)), (int)(y0 + 1000 * (a)));
            Point endPoint = new Point((int)(x0 - 1000 * (-b)), (int)(y0 - 1000 * (a)));
            return new Line(startPoint, endPoint);  
        }

        public IEnumerable<Line> ExtractGeometry(Stream imageStream)
        {
            Mat image = Mat.FromStream(imageStream, ImreadModes.Grayscale);
            
            Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(3, 3), 0);

            // Бінаризація зображення для відсіювання шуму та отримання чітких контурів
            Cv2.Threshold(image, image, 127, 255, ThresholdTypes.Binary);
            (double low, double high) = CannyParameters(image);
            // Застосування детектора країв Canny
            Cv2.Canny(image, image, low, high);

            // Виявлення ліній за допомогою алгоритму Хафа
            LineSegmentPolar[] lines = Cv2.HoughLines(image, 1, Math.PI / 180, 150);
            return lines.Select(l => FormatLine(l)).ToArray();
        }
    }
}
