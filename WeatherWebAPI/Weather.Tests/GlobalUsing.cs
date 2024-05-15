global using System.Security.Claims;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Options;

global using AutoMapper;
global using FluentAssertions;
global using Moq;

global using Weather.Application.DataContracts.Responses;
global using Weather.Application.Map;
global using Weather.Application.Services;

global using Weather.DataAccess;
global using Weather.DataAccess.Repositories.Base;

global using Weather.Domain.Clients;
global using Weather.Domain.Dtos;
global using Weather.Domain.Entities;
global using Weather.Domain.Repositories;

global using Weather.WebApi.Controllers;