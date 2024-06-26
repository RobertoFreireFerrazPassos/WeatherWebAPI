﻿global using System.Security.Claims;
global using System.Net;
global using System.Text;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;

global using AutoMapper;
global using FluentAssertions;
global using Moq;
global using Newtonsoft.Json;

global using Weather.Application.DataContracts.Requests;
global using Weather.Application.DataContracts.Responses;
global using Weather.Application.Map;
global using Weather.Application.Services;

global using Weather.BaseClient;

global using Weather.DataAccess;
global using Weather.DataAccess.Repositories.Base;

global using Weather.Domain.Clients;
global using Weather.Domain.Dtos;
global using Weather.Domain.Entities;
global using Weather.Domain.Repositories;

global using Weather.Redis;

global using Weather.Tests.Utils;

global using Weather.WebApi.Controllers;