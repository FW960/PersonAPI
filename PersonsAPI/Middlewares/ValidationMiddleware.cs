using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Services.Validators.Companies;
using EmployeesAPI.Services.Validators.Customers;
using EmployeesAPI.Services.Validators.Employees;
using EmployeesAPI.Services.Validators.ErrorCodes;

namespace EmployeesAPI.Middlewares;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ValidationCodes _validationCodes = ValidationCodes.Create();

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        byte[] buffer = new byte[1024];

        switch (context.Request.Path)
        {
            case "/employees/add/agent":
            {
                int bytes = await context.Request.Body.ReadAsync(buffer);

                string json = Encoding.UTF8.GetString(buffer, 0, bytes);

                EmployeeDTO? dto;

                try
                {
                    dto = JsonSerializer.Deserialize<EmployeeDTO>(json);

                    EmployeesValidator validator = new EmployeesValidator();

                    var result = validator.Validate(dto);

                    if (result.IsValid)
                    {
                        break;
                    }
                    else
                    {
                        string validationErrorMessage =
                            _validationCodes.GetCodeMessage(result.Errors.First().ErrorCode);

                        context.Response.StatusCode = 422;

                        await context.Response.WriteAsJsonAsync(validationErrorMessage);

                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = 400;
                    return;
                }
            }
            case "/customers/add/agent":
            {
                int bytes = await context.Request.Body.ReadAsync(buffer);

                string json = Encoding.UTF8.GetString(buffer, 0, bytes);

                CustomerDto? dto;

                try
                {
                    dto = JsonSerializer.Deserialize<CustomerDto>(json);

                    CustomersValidator validator = new CustomersValidator();

                    var result = validator.Validate(dto);

                    if (result.IsValid)
                    {
                        break;
                    }
                    else
                    {
                        string validationErrorMessage =
                            _validationCodes.GetCodeMessage(result.Errors.First().ErrorCode);

                        context.Response.StatusCode = 422;

                        await context.Response.WriteAsJsonAsync(validationErrorMessage);

                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = 400;
                    return;
                }
            }
            case "/company/add":
            {
                int bytes = await context.Request.Body.ReadAsync(buffer);

                string json = Encoding.UTF8.GetString(buffer, 0, bytes);

                CompanyDto? dto;

                try
                {
                    dto = JsonSerializer.Deserialize<CompanyDto>(json);

                    CompanyValidator validator = new CompanyValidator();

                    var result = validator.Validate(dto);

                    if (result.IsValid)
                    {
                        break;
                    }
                    else
                    {
                        string validationErrorMessage =
                            _validationCodes.GetCodeMessage(result.Errors.First().ErrorCode);

                        context.Response.StatusCode = 422;

                        await context.Response.WriteAsJsonAsync(validationErrorMessage);

                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = 400;
                    return;
                }
            }
        }

        await _next(context);
    }
}