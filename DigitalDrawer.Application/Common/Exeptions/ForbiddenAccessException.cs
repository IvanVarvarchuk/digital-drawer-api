using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Application.Common.Exeptions;
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
}
