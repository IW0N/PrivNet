using Server.RequestHandlers;
using Server;
using Server.Middlewares.Extensions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Routing.Patterns;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();
var app = builder.Build();
app.UseAliasChecker();
app.MapPost("/api/user",SignupHandler.SignUp);
app.MapGet("/api/user",);
app.MapPost("/api/user/chat",ChatHandler.Create);
app.MapGet("/api/user/update",UpdateHandler.GetUpdate);
app.MapDelete("/api/user/update",UpdateHandler.DeleteUpdate);
app.MapGet("/api/users",UserHandler.FindUsers);
app.Run();