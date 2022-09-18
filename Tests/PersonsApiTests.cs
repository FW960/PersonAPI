using AuthorizationAPI.Services.Services;
using AutoMapper;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Persons;
using EmployeesAPI.Repositories.Persons.Interfaces;
using EmployeesAPI.Services.Services;
using EmployeesAPI.Services.Validators.Employees;
using Moq;
using Xunit;

namespace Tests;

public class PersonsApiTests
{
    [Theory]
    [InlineData("101", 0)]
    [InlineData("102", 1)]
    [InlineData("103", 2)]
    public void PersonsValidationCodesTest(string code, int count)
    {
        EmployeesValidator employeesValidator = new EmployeesValidator();

        var result = employeesValidator.Validate(new EmployeeDTO
        {
            Age = 12,
            Email = "incorrectEmailTest",
            FirstName = "",
            LastName = "LastName",
            Group = 1
        });

        var errors = result.Errors;
        
        Assert.True(errors[count].ErrorCode.Equals(code));
    }
}