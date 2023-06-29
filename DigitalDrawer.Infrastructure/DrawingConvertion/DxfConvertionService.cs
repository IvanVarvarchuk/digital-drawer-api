﻿using DigitalDrawer.Application.Common.Helpers;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Infrastructure.DrawingConvertion
{
    public class DxfConvertionService : IFileConversionService
    {
        public byte[] Convert(IEnumerable<Line> lines)
        {
            var writer = new DxfWriter();

            return writer.WriteLines(lines);
        }
    }

    internal class DxfWriter
    {
        public byte[] WriteLines(IEnumerable<Line> lines)
        {
            using var memoryStream = new MemoryStream();

            using var writer = new StreamWriter(memoryStream);
                foreach (var line in lines)
                {
                    writer.WriteLine($"0\nLINE");
                    writer.WriteLine($"8\n0"); // Layer name (0 is default layer)
                    writer.WriteLine($"10\n{line.Start.X:F6}");
                    writer.WriteLine($"20\n{line.Start.Y:F6}");
                    writer.WriteLine($"11\n{line.Start.X:F6}");
                    writer.WriteLine($"21\n{line.Start.X:F6}");
                }

                writer.WriteLine($"0\nEOF");
                memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ReadAllBytes();
        }
    }


}
