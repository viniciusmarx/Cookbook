using Cookbook.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Attributes;

public class AuthenticatedUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserFilter)) { }