using CatchIQ.API.Data;
using CatchIQ.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json.Serialization;
using CatchIQ.API.Engines.Interfaces;
using CatchIQ.API.Engines;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Managers;
using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Accessors;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

///
/// DI
/// 

// Accessors
builder.Services.AddScoped<ISpeciesAccessor, SpeciesAccessor>();
builder.Services.AddScoped<ICatchAccessor, CatchAccessor>();
builder.Services.AddScoped<ISpotAccessor, SpotAccessor>();
builder.Services.AddScoped<IBaitAccessor, BaitAccessor>();

// Managers
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<ISpeciesManager, SpeciesManager>();
builder.Services.AddScoped<ICatchManager, CatchManager>();
builder.Services.AddScoped<ISpotManager, SpotManager>();
builder.Services.AddScoped<IBaitManager, BaitManager>();

// Engines
builder.Services.AddScoped<ITokenEngine, TokenEngine>();

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});

// Serialize enums as their names ("SoftPlastic") rather than ints, so the OpenAPI
// spec carries the names and NSwag can generate string-literal union types.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Emit operationIds as "{Controller}_{Action}" so NSwag can split them into
    // one client class per controller with clean method names (BaitClient.getAll()).
    c.CustomOperationIds(apiDescription =>
        apiDescription.ActionDescriptor is ControllerActionDescriptor descriptor
            ? $"{descriptor.ControllerName}_{descriptor.ActionName}"
            : null);

    // Treat non-nullable C# properties as `required` in the schema, so NSwag
    // generates `id: number` rather than `id?: number | undefined`.
    c.SupportNonNullableReferenceTypes();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();