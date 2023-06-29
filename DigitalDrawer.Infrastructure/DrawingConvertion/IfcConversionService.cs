using DigitalDrawer.Application.Common.Helpers;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Infrastructure.DrawingConvertion
{
    public class IfcConversionService : IFileConversionService
    {
        
        public byte[] Convert(IEnumerable<Line> lines)
        {
            var ifcWriter = new IFCWriter();
            return ifcWriter.CreateIFCFile(FormatLines(lines));
        }

        private static IEnumerable<IfcLine> FormatLines(IEnumerable<Line> lines)
        {
            int startId = 0; int endId = 1;
            return lines.Select((l, i) => new IfcLine()
            {
                EndPointId = endId + i,
                StartPointId = startId + i,
                StartPoint = new Point3D(l.Start),
                EndPoint = new Point3D(l.End),
            });
        }

    }

    public class IFCWriter
    {
        public byte[] CreateIFCFile(IEnumerable<IfcLine> lines)
        {

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            try
            {
                
                WriteHeader(writer);
                WriteData(writer, lines);
                memoryStream.Seek(0, SeekOrigin.Begin);

                Console.WriteLine("IFC file created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating IFC file: " + ex.Message);
            }

            return memoryStream.ReadAllBytes();
        }

        private void WriteHeader(StreamWriter writer)
        {
            writer.WriteLine("ISO-10303-21;");
            writer.WriteLine("HEADER;");
            writer.WriteLine("FILE_DESCRIPTION(('ViewDefinition [CoordinationView]'), '2;1');");
            writer.WriteLine("FILE_NAME('example.ifc', '2023-05-26T00:00:00', (''), (''), 'IFC Writer', '', '', '');");
            writer.WriteLine("FILE_SCHEMA(('IFC2X3'));");
            writer.WriteLine("ENDSEC;");
        }

        private void WriteData(StreamWriter writer, IEnumerable<IfcLine> lines)
        {
            writer.WriteLine("DATA;");
            writer.WriteLine("/* VERTICES */");

            foreach (var line in lines)
            {
                writer.WriteLine($"#{line.StartPointId} = IFCCARTESIANPOINT({line.StartPoint.X}, {line.StartPoint.Y}, {line.StartPoint.Z});");
                writer.WriteLine($"#{line.EndPointId} = IFCCARTESIANPOINT({line.EndPoint.X}, {line.EndPoint.Y}, {line.EndPoint.Z});");
            }

            writer.WriteLine("/* LINES */");

            foreach (var line in lines)
            {
                writer.WriteLine($"#1 = IFCPOLYLINE(('','Polyline',''), #{line.StartPointId}, #{line.EndPointId});");
            }

            writer.WriteLine("ENDSEC;");
            writer.WriteLine("END-ISO-10303-21;");
        }
    }

    public class IfcLine
    {
        public int StartPointId { get; set; }
        public Point3D StartPoint { get; set; }
        public int EndPointId { get; set; }
        public Point3D EndPoint { get; set; }
    }

    public class Point3D
    {
        public Point3D(Point point): this(point.X, point.Y) { }

        public Point3D(double x, double y) : this(x, y, 0d) { }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }


        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
