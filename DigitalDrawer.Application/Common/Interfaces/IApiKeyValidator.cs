namespace DigitalDrawer.Application.Common.Interfaces;

public interface IApiKeyValidator
{
    bool IsValid(string apiKey);
}
