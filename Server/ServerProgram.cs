using Server.RequestHandlers;
using Server;
using Microsoft.Extensions.Options;
using Server.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();
var app = builder.Build();
app.MapPost("/api/user",SignupHandler.SignUp);
app.MapPost("/api/user/chat",ChatHandler.Create);
app.MapGet("/api/user/update",UpdateHandler.GetUpdate);
//app.MapDelete("/api/update",UpdateHandler.);
app.Run();