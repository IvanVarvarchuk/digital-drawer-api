using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.MediatR;

public static class HangfireConfigurationExtention
{
    public static void UseMediatR(this IGlobalConfiguration configuration)
    {
        var jsonSetting = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        configuration.UseSerializerSettings(jsonSetting);
    }
}
