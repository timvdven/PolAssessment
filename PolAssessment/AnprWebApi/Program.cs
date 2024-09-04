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
    .AddCors(x =>
        x.AddPolicy("AllowSpecificOrigins", y => y
            .WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials())
    )
    .AddEndpointsApiExplorer()
    .RegisterSwagger()
    .RegisterAnprDbContext(builder.Configuration)
    .RegisterWebApiDbContext(builder.Configuration)
    .AddScoped<IAuthTokenHandler, AuthTokenHandler>()
    .AddScoped<IAnprQueryService, AnprQueryService>()
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

app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
