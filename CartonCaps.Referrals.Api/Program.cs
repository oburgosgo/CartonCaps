using CartonCaps.Referrals.Api.Authentication;
using CartonCaps.Referrals.Api.Middlewares;
using CartonCaps.Referrals.Application;
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Options;
using CartonCaps.Referrals.Infrastructure;
using CartonCaps.Referrals.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddOptions<ReferralInviteOptions>()
    .Bind(builder.Configuration.GetSection("ReferralInvites"));

builder.Services.AddOptions<ReferralShareOptions>()
    .Bind(builder.Configuration.GetSection("ReferralShare"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(configuration => {

    configuration.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CartonCaps.Referrals API",
        Version = "v1",
        Description = "Referral invites API (Create / Resolve / Redeem) referral invites."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        configuration.IncludeXmlComments(xmlPath);

    configuration.AddSecurityDefinition("DebugUser", new OpenApiSecurityScheme
    {
        Name = "X-Debug-UserId",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Mock authenticated user id. Example: oburgosgo@gmail.com"
    });

    configuration.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("DebugUser", document)] = new List<string>()
    });

});

builder.Services
    .AddAuthentication("Fake")
    .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>("Fake", _ => { });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddTransient<GlobalExceptionMiddleware>();

builder.Services.AddControllers(o =>
{
    o.AllowEmptyInputInBodyModelBinding = true;
})
.ConfigureApiBehaviorOptions(o =>
{
    o.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

Directory.CreateDirectory("data");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
