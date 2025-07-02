global using System.Net.Http.Headers;
global using System.Security.Claims;
global using System.Text.Encodings.Web;
global using System.Text;
global using System.ComponentModel.DataAnnotations;

global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;

global using AutoMapper;

global using Weather.CrossCutting.IoC;

global using Weather.WebApi.DataContracts.Requests;
global using Weather.WebApi.DataContracts.Responses;
global using Weather.WebApi.Middlewares;
global using Weather.WebApi.Handlers;

global using Weather.Application.Interfaces.Services;
global using Weather.Application.Interfaces.Repositories;
global using Weather.Application.Interfaces.Security;
global using Weather.Application.Dtos;
global using Weather.Application.Map;

global using Weather.BaseClient;

global using Weather.DataAccess;

global using Weather.Domain.Entities;

global using Weather.Redis;