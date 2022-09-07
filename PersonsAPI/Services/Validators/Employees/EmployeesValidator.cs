using EmployeesAPI.DTOs;
using FluentValidation;

namespace EmployeesAPI.Services.Validators.Employees;

public class EmployeesValidator : AbstractValidator<EmployeeDTO>
{
    public EmployeesValidator()
    {
        RuleFor(x => x.Age).GreaterThan(16).WithErrorCode("101").LessThan(80).WithErrorCode("101");
        RuleFor(x => x.Email).EmailAddress().WithErrorCode("102");
        RuleFor(x => x.FirstName).NotEmpty().WithErrorCode("103").NotNull().WithErrorCode("103");
        RuleFor(x => x.LastName).NotEmpty().WithErrorCode("103").NotNull().WithErrorCode("103");
        RuleFor(x => x.FirstName).Matches("[a-z]").WithErrorCode("104").Matches("[A-Z]").WithErrorCode("104");
        RuleFor(x => x.LastName).Matches("[a-z]").WithErrorCode("104").Matches("[A-Z]").WithErrorCode("104");
    }
}