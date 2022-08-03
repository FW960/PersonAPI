using System.Text;
using System.Web.Http;
using AutoMapper;
using MySqlConnector;
using NLog;
using NLog.Web;
using EmployeesAPI.DatabaseContext;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Middlewares;
using EmployeesAPI.Repositories.Persons;
using EmployeesAPI.Services;
using EmployeesAPI.Services.Persons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    MapperConfiguration mapperConfig = new MapperConfiguration(conf => conf.CreateMap<PersonDTO, Person>());

    Mapper mapperFromDto = new Mapper(mapperConfig);

    mapperConfig = new MapperConfiguration(conf => conf.CreateMap<Person, PersonDTO>());

    Mapper mapperToDto = new Mapper(mapperConfig);

    string connectionString = builder.Configuration.GetConnectionString("default");

    MySqlConnection connection = new MySqlConnection {ConnectionString = connectionString};

    PersonsServices services = new PersonsServices(mapperFromDto, new PersonsRepository(connection), mapperToDto);

    builder.Services.AddDbContext<PersonsDbContext>();

    builder.Services.AddSingleton(services);

    builder.Services.AddControllers();

    builder.Services.AddSwaggerGen();

    #region Swagger Configuration

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

    #endregion

    #region Authentication

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        }
    );
    builder.Services.AddAuthorization();

    #endregion

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();
    
    app.UseAuthentication();

    app.UseMiddleware<TokenValidationMiddleware>();

    app.MapGet("/", [Authorize]() => "Hello World!");

    app.UseRouting();
    
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        ));

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}