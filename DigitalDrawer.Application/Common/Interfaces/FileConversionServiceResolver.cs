using DigitalDrawer.Domain.Enums;

namespace DigitalDrawer.Application.Common.Interfaces;

public delegate IFileConversionService FileConversionServiceResolver(TargetFileFormat key);

