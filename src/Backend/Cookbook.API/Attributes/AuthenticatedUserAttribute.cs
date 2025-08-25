using Cookbook.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticatedUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserFilter)) { }