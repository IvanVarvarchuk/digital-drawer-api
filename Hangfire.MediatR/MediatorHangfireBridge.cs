using MediatR;
using System.ComponentModel;

namespace Hangfire.MediatR;

public class MediatorHangfireBridge
{
    private readonly ISender _mediator;
    
    public MediatorHangfireBridge(ISender mediator)
    {
        _mediator = mediator;
    }
    [DisplayName("{0}")]
    public async Task Send(string jobName, IRequest request)
    {
        await _mediator.Send(request);
    }
    
    [DisplayName("{0}")]
    public async Task Send<TResponse>(string jobName, IRequest<TResponse> request)
    {
        await _mediator.Send(request);
    }
}