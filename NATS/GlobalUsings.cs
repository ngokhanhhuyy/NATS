global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Globalization;
global using System.Security.Claims;
global using System.Text;
global using System.Reflection;
global using System.IdentityModel.Tokens.Jwt;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.IdentityModel.Tokens;

global using NATS.Extensions;
global using NATS.Models;
global using NATS.Services;
global using NATS.Services.Entities;
global using NATS.Services.Extensions;
global using NATS.Services.Localization;
global using NATS.Services.Results;
global using NATS.Services.Interfaces;
global using NATS.Services.Handlers;
global using NATS.Services.Identity;
global using NATS.Services.Validations;
global using NATS.Services.Validations.Validators;
global using NATS.Services.Enums;
global using NATS.Services.Dtos.RequestDtos;
global using NATS.Services.Dtos.ResponseDtos;

global using FluentValidation;
global using FluentValidation.Results;
global using ValidationResult = FluentValidation.Results.ValidationResult;
global using ValidationFailure = FluentValidation.Results.ValidationFailure;

global using ImageMagick; 