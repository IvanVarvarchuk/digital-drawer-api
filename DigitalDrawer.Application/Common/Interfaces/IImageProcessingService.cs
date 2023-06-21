using DigitalDrawer.Application.Common.Models.Geometry;

namespace DigitalDrawer.Application.Common.Interfaces;

public interface IImageProcessingService
{
    IEnumerable<Line> ExtractGeometry(Stream imageStream);
}
