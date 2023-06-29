using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Infrastructure.ImageProcessing;

public class CannyParameters
{
    public double HighThreshold { get; set; }
    public double LowThreshold { get; set; }
}

public static class CannyParameterSelector
{
    public static CannyParameters SelectParameters(this Mat image)
    {
        // Крок 1: Обчислення градієнтів
        Mat dx = new Mat();
        Mat dy = new Mat();
        Cv2.Sobel(image, dx, MatType.CV_32F, 1, 0);
        Cv2.Sobel(image, dy, MatType.CV_32F, 0, 1);

        // Крок 2: Обчислення магнітуди та напрямку градієнтів
        Mat magnitude = new Mat();
        Mat angle = new Mat();
        Cv2.CartToPolar(dx, dy, magnitude, angle, true);

        // Крок 3: Обчислення гістограми градієнтів
        int[] histSize = { 256 };
        Rangef[] ranges = { new Rangef(0, 256) };
        int[] channels = { 0 };
        Mat hist = new Mat();
        Cv2.CalcHist(new[] { magnitude }, channels, null, hist, 1, histSize, ranges);

        // Крок 4: Вибір порогів з високою кількістю градієнтів
        double highThreshold = 0;
        double lowThreshold = 0;
        double highGradientCountThreshold = 0.1; // Поріг кількості градієнтів

        for (int i = histSize[0] - 1; i >= 0; i--)
        {
            double binValue = hist.At<float>(i);

            if (highThreshold == 0 && binValue >= highGradientCountThreshold * hist.Rows)
            {
                highThreshold = ranges[0].End + (i * (ranges[0].End - ranges[0].Start) / histSize[0]);
            }
            else if (highThreshold != 0 && lowThreshold == 0 && binValue < highGradientCountThreshold * hist.Rows)
            {
                lowThreshold = ranges[0].Start + (i * (ranges[0].End - ranges[0].Start) / histSize[0]);
                break;
            }
        }

        // Переконайтеся, що значення порогів встановлені
        if (highThreshold == 0)
            highThreshold = 0.7 * histSize[0];

        if (lowThreshold == 0)
            lowThreshold = 0.3 * histSize[0];

        return new CannyParameters
        {
            HighThreshold = highThreshold,
            LowThreshold = lowThreshold
        };
    }
}
