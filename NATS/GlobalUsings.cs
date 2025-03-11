global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Globalization;
global using System.Security.Claims;
global using System.Text;
global using System.Reflection;

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
global using NATS.Services.Options;
global using NATS.Services.Identity;
global using NATS.Services.Validations;
global using NATS.Services.Validations.Validators;
global using NATS.Services.Enums;
global using NATS.Services.Dtos;
global using NATS.Services.Dtos.RequestDtos;
global using NATS.Services.Dtos.ResponseDtos;

global using MySqlConnector;
global using FluentValidation;
global using ValidationResult = FluentValidation.Results.ValidationResult;
global using ValidationFailure = FluentValidation.Results.ValidationFailure;

global using ImageMagick;
global using Bogus;