using DigitalDrawer.Application;
using DigitalDrawer.Infrastructure;
using Hangfire;
using Hangfire.MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.AddHangfire(c =>
{
    c.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
    c.UseMediatR();
});
builder.Services.AddHangfireServer();

var app = builder.Build();


app.Run();
