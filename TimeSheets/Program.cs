using System.Data.Common;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("default");

builder.Services.AddSingleton(new MySqlConnection {ConnectionString = connectionString});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoints => 
    endpoints.MapControllerRoute(name:"default", pattern:"{controller=Home}/{action=Index}/{id?}"));

app.Run();