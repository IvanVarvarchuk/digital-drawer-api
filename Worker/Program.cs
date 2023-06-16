using Hangfire;
using Hangfire.MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHangfire(c =>
{
    c.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
    c.UseMediatR();
});
builder.Services.AddHangfireServer();

var app = builder.Build();


app.Run();
