// See https://aka.ms/new-console-template for more information

using AutoMapper;
using PersonsAPI.DTOs;
using PersonsAPI.Entities;

var conf = new MapperConfiguration(configure => configure.CreateMap<PersonDTO, Person>());

Mapper map = new Mapper(conf);

var a = map.Map<Person>(new PersonDTO());

Console.Write("A");