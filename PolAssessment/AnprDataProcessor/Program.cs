using PolAssessment.AnprDataProcessor.Extensions;
using PolAssessment.AnprDataProcessor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug();

builder.Services
    .AddEndpointsApiExplorer()
    .RegisterSwagger()
    .RegisterDbContext(builder.Configuration)
    .AddScoped<IAuthTokenHandler, AuthTokenHandler>()
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
