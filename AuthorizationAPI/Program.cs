using AuthorizationAPI.Services.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Admin Cors",
        policy =>
        {
            policy.WithOrigins("https://localhost:7292")
                .AllowAnyHeader()
                .AllowAnyMethod().Build();
        });
    options.AddPolicy("Employee Cors",
        policy => { policy.WithOrigins("https://localhost:7230").AllowAnyHeader().AllowAnyMethod().Build(); });
});

builder.Services.AddSingleton(new EmployeeService(new MySqlConnection
    {ConnectionString = builder.Configuration.GetConnectionString("default")}));

builder.Services.AddSingleton(new AdminService(new MySqlConnection
    {ConnectionString = builder.Configuration.GetConnectionString("default")}));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.UseRouting();

app.UseCors("Employee Cors");

app.UseCors("Admin Cors");

app.UseEndpoints(endpoints =>
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    ));

app.Run();