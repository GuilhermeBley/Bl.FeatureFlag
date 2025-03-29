using Bl.FeatureFlag.Api.Endpoints;
using Bl.FeatureFlag.Api.Middlewares;
using Bl.FeatureFlag.Api.Services;
using Bl.FeatureFlag.Infrastructure.Di;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

FlagEndpoints.MapEndpoints(app);
IdentityEndpoints.MapEndpoints(app);

app.Run();
