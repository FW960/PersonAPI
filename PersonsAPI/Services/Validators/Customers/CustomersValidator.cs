using System.Data;
using EmployeesAPI.DTOs;
using FluentValidation;

namespace EmployeesAPI.Services.Validators.Customers;

public class CustomersValidator : AbstractValidator<CustomerDto>
{
    public CustomersValidator()
    {
        RuleFor(x => x.Post).Matches("[a-z]").WithErrorCode("201").Matches("[A-Z]").WithErrorCode("201");
        RuleFor(x => x.CompanyInn).Matches("[0-9]").WithErrorCode("202");
        RuleFor(x => x.Email).EmailAddress().WithErrorCode("102");
        RuleFor(x => x.FirstName).NotEmpty().WithErrorCode("103").NotNull().WithErrorCode("103");
        RuleFor(x => x.LastName).NotEmpty().WithErrorCode("103").NotNull().WithErrorCode("103");
        RuleFor(x => x.FirstName).Matches("[a-z]").WithErrorCode("104").Matches("[A-Z]").WithErrorCode("104");
        RuleFor(x => x.LastName).Matches("[a-z]").WithErrorCode("104").Matches("[A-Z]").WithErrorCode("104");
    }
}