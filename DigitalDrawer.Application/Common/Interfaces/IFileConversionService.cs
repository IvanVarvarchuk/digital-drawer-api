using DigitalDrawer.Application.Common.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Application.Common.Interfaces;

public interface IFileConversionService
{
    byte[] Convert(IEnumerable<Line> lines);
}
