global using System.Net;
global using System.Net.Http.Headers;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Text.Encodings.Web;
global using System.Text;

global using Microsoft.Extensions.Options;

global using Microsoft.AspNetCore.Authentication;
global using Microsoft.OpenApi.Models;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;

global using AutoMapper;

global using Weather.CrossCutting.IoC;

global using Weather.Application.DataContracts.Requests;
global using Weather.Application.DataContracts.Responses;

global using Weather.BaseClient;

global using Weather.DataAccess;

global using Weather.Domain.Dtos;
global using Weather.Domain.Repositories;
global using Weather.Domain.Services;

global using Weather.Redis;

global using Weather.Security;

global using Weather.WebApi;