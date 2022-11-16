using Server.RequestHandlers;
using Server;
using Server.Middlewares.Extensions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Routing.Patterns;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();
var app = builder.Build();
app.MapPost("/api/user", SignupHandler.SignUp);
app.MapWhen(c => c.Request.Path != "/api/user", builder =>
{
    builder.UseAliasChecker();
    builder.UseUserExtractor();
    builder.UseRequestDecryptor();

    //builder.UseMapGet("/api/user",);
    builder.UseMapPost("/api/user/chat", ChatHandler.Create);
    builder.UseMapGet("/api/user/update", UpdateHandler.GetUpdate);
    builder.UseMapDelete("/api/user/update", UpdateHandler.DeleteUpdate);
    builder.UseMapGet("/api/users", UserHandler.FindUsers);
    builder.UseTemporaryUpdater();
    builder.RunEncryption();
});
app.Run();