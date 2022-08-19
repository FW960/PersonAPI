using AutoMapper.Configuration.Conventions;
using EmployeesAPI.Entities;

namespace EmployeesAPI.DTOs;

public class CustomerDto : BasePersonDTO
{
    public string Post { get; set; }
    public CompanyDto Company { get; set; }
}