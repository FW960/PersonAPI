// See https://aka.ms/new-console-template for more information

using System.Text;
using AutoMapper;
using EmployeesAPI.DatabaseContext;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Services.Persons;


string encryptedPass = (Encrypt.Password("24beluro"));

Console.Write(encryptedPass);