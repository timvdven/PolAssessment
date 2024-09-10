using PolAssessment.AnprDataProcessor.WebApi.DbContexts;
using PolAssessment.AnprDataProcessor.WebApi.Services;
using PolAssessment.Common.Lib.Configuration;
using PolAssessment.Common.Lib.Extensions;
using PolAssessment.Common.Lib.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettingsSecrets.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddConfiguration(configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug();

builder.Services
    .AddEndpointsApiExplorer()
    .RegisterSwagger("v1", "ANPR Data Processor API")
    .RegisterDbContext<IAnprDataDbContext, AnprDataDbContext>(configuration.GetSection("Database").Get<DatabaseConfig>()!)

    .Configure<JwtConfig>(configuration.GetSection("Jwt"))
    .AddScoped<IAuthTokenHandler, AuthTokenHandler>()
    .AddScoped<IHashService, HashService>()
    
    .RegisterJwt(configuration.GetSection("Jwt").Get<JwtConfig>()!)

    .AddScoped<IAnprControllerService, AnprControllerService>()
    .AddScoped<IAuthorizeControllerService, AuthorizeControllerService>()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
