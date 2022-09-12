using ContractsAPI.Middlewares;
using ContractsAPI.Repositories;
using ContractsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = "ASP.NET Core 5.0 Web API"
    });
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = EmployeeAuthOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = EmployeeAuthOptions.AUDIENCE,
        ValidateLifetime = true,
        IssuerSigningKey = EmployeeAuthOptions.GetSymmetricSecurityKey(true),
        ValidateIssuerSigningKey = true
    }
);
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("default");

MySqlConnection connection = new MySqlConnection(connectionString);

ContractRepository repository = new ContractRepository(connection);

ContractServices services = new ContractServices(repository);

builder.Services.AddSingleton(services);

var app = builder.Build();

app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.UseSwagger();

app.UseSwaggerUI();

app.UseMiddleware<AuthorizationMiddleware>();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    ));

app.Run();