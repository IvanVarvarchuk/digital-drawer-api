using DigitalDrawer.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDrawer.WebAPI.Auth.ApiKey;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}