using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.AnprWebApi.Services;
using PolAssessment.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug();

builder.Services
    .AddEndpointsApiExplorer()
    .RegisterSwagger()
    .RegisterAnprDbContext(builder.Configuration)
    .RegisterWebApiDbContext(builder.Configuration)
    .AddScoped<IAuthTokenHandler, AuthTokenHandler>()
    .AddScoped<IHashService, HashService>()
    .RegisterJwt(builder.Configuration)
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
