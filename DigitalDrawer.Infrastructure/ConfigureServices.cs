using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Domain.Enums;
using DigitalDrawer.Infrastructure.DrawingConvertion;
using DigitalDrawer.Infrastructure.Identity;
using DigitalDrawer.Infrastructure.ImageProcessing;
using DigitalDrawer.Infrastructure.Persistance;
using DigitalDrawer.Infrastructure.Persistance.Interseptors;
using DigitalDrawer.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalDrawer.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("DigitalDrawerDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        //services.AddScoped<ApplicationDbContextInitialiser>();

        services
            .AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();


        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IAccesTokenService, JwtService>();
        services.AddTransient<DxfConvertionService>();
        services.AddTransient<IfcConversionService>();
        services.AddTransient<SvgConvertionService>();
        services.AddTransient<IImageProcessingService, ImageProcessingService>();
        _ = services.AddTransient<FileConversionServiceResolver>(serviceProvider => key =>
        {
            IFileConversionService? value = key switch
            {
                TargetFileFormat.DXF => serviceProvider.GetService<DxfConvertionService>(),
                TargetFileFormat.IFC => serviceProvider.GetService<IfcConversionService>(),
                TargetFileFormat.SVG => serviceProvider.GetService<SvgConvertionService>(),
                _ => throw new KeyNotFoundException(), // or maybe return null, up to you
            };
            return value!;
        });
        return services;
    }
}