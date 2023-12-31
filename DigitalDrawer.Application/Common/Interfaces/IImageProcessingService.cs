﻿using DigitalDrawer.Application.Common.Models.Geometry;

namespace DigitalDrawer.Application.Common.Interfaces;

public interface IImageProcessingService
{
    Task<Line[]> ExtractLines(Stream imageStream);
}
