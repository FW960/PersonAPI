using EmployeesAPI.DTOs;
using FluentValidation;

namespace EmployeesAPI.Services.Validators.Companies;

public class CompanyValidator : AbstractValidator<CompanyDto>
{
    public CompanyValidator()
    {
        RuleFor(x => x.Inn).Matches("[0-9]").WithErrorCode("202");
    }
}