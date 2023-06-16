using MediatR;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.MediatR;

public static class MediatorExtentions
{
    public static string Schedule<TResponse>(this ISender mediator, string jobName, IRequest<TResponse> request, DateTimeOffset dateTimeOffset)
    { 
        var backgrounJobClient = new BackgroundJobClient();
        return backgrounJobClient.Schedule<MediatorHangfireBridge>(x => x.Send(jobName, request), dateTimeOffset);
    }
    public static string Schedule<TResponse>(this ISender mediator, string jobName, IRequest<TResponse> request, TimeSpan timeSpan)
    { 
        var backgrounJobClient = new BackgroundJobClient();
        return backgrounJobClient.Schedule<MediatorHangfireBridge>(x => x.Send(jobName, request), timeSpan);
    }
    public static string Enqueue<TResponse>(this ISender mediator, string jobName, IRequest<TResponse> request) 
    {
        var backgrounJobClient = new BackgroundJobClient();
        return backgrounJobClient.Enqueue<MediatorHangfireBridge>(x => x.Send<TResponse>(jobName, request));
    }
}
