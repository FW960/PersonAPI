using AutoMapper.Configuration.Conventions;
using EmployeesAPI.Entities;

namespace EmployeesAPI.DTOs;

public class CustomerDto : BasePersonDTO
{
    public string Post { get; set; }
    public string CompanyInn { get; set; }
}