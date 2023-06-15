using DigitalDrawer.Application.Common.Interfaces;

namespace DigitalDrawer.WebAPI.Services;

public class ApiKeyValidator: IApiKeyValidator
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ApiKeyValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public bool IsValid(string apiKey)
    {

        var result = _context.ApiKeys.SingleOrDefault(x => x.Key == apiKey);

        return result != null;
    }
}
