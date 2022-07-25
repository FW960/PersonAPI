using AutoMapper;
using MySqlConnector;
using NLog;
using NLog.Web;
using PersonsAPI.DTOs;
using PersonsAPI.Entities;
using PersonsAPI.Repositories.Persons;
using PersonsAPI.Services.Persons;

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

    builder.Services.AddSingleton(services);

    builder.Services.AddControllers();

    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.MapGet("/", () => "Hello World!");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();

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