global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Globalization;
global using System.Security.Claims;
global using System.Text;
global using System.Reflection;
global using System.Diagnostics.CodeAnalysis;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Mvc.ModelBinding;

global using NATS.Extensions;
global using NATS.Middlewares;
global using NATS.Models;
global using NATS.Services;
global using NATS.Services.Entities;
global using NATS.Services.Extensions;
global using NATS.Services.Exceptions;
global using NATS.Services.Localization;
global using NATS.Services.Interfaces;
global using NATS.Services.Handlers;
global using NATS.Services.Identity;
global using NATS.Services.Enums;
global using NATS.Services.Dtos;
global using NATS.Services.Dtos.RequestDtos;
global using NATS.Services.Dtos.ResponseDtos;
global using NATS.Validation;
global using NATS.Validation.Validators;

global using IAuthenticationService = NATS.Services.Interfaces.IAuthenticationService;
global using IAuthorizationService = NATS.Services.Interfaces.IAuthorizationService;
global using AuthenticationService = NATS.Services.AuthenticationService;
global using MySqlConnector;
global using FluentValidation;
global using ValidationResult = FluentValidation.Results.ValidationResult;
global using ValidationFailure = FluentValidation.Results.ValidationFailure;

global using ImageMagick;
global using Bogus;
global using JetBrains.Annotations;