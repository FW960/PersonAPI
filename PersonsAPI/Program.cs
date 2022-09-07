using System.Web.Http;
using AutoMapper;
using MySqlConnector;
using NLog;
using NLog.Web;
using EmployeesAPI.DatabaseContext;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Middlewares;
using EmployeesAPI.Repositories;
using EmployeesAPI.Repositories.Persons;
using EmployeesAPI.Services.Services;
using EmployeesAPI.Services.Validators.Companies;
using EmployeesAPI.Services.Validators.Customers;
using EmployeesAPI.Services.Validators.Employees;
using EmployeesAPI.Services.Validators.ErrorCodes;
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

    MapperConfiguration employeeMapperConfig = new MapperConfiguration(conf => conf.CreateMap<EmployeeDTO, Employee>());

    Mapper employeeMapperFromDto = new Mapper(employeeMapperConfig);

    employeeMapperConfig = new MapperConfiguration(conf => conf.CreateMap<Employee, EmployeeDTO>());

    Mapper employeeMapperToDto = new Mapper(employeeMapperConfig);
    
    string connectionString = builder.Configuration.GetConnectionString("default");

    MySqlConnection connection = new MySqlConnection {ConnectionString = connectionString};

    EmployeeServices employeeServices =
        new EmployeeServices(employeeMapperFromDto, new EmployeeRepository(connection), employeeMapperToDto);

    CustomersServices customersServices =
        new CustomersServices(new CustomerRepository(connection));

    CompanyServices companyServices = new CompanyServices(new CompanyRepository(connection));

    builder.Services.AddDbContext<PersonsDbContext>();

    builder.Services.AddScoped<CompanyValidator>();

    builder.Services.AddScoped<EmployeesValidator>();

    builder.Services.AddScoped<CustomersValidator>();

    builder.Services.AddSingleton(ValidationCodes.Create());

    builder.Services.AddSingleton(employeeServices);

    builder.Services.AddSingleton(customersServices);

    builder.Services.AddSingleton(companyServices);

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
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(true),
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

    app.UseMiddleware<ValidationMiddleware>();

    app.UseMiddleware<AuthMiddleware>();

    app.UseStaticFiles();

    app.UseAuthentication();

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