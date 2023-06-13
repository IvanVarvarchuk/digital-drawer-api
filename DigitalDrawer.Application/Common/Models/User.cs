using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Application.Common.Models;

public record User
{
    public string Id { get; init; }
    public string UserName { get; init; }
    public string Email { get; init; }
}
