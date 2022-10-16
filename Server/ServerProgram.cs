using Server.RequestHandlers;
using Server;
using Microsoft.Extensions.Options;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();
var app = builder.Build();

app.MapPost("/api/user",SignupHandler.SignUp);
app.MapPost("/api/user/chat",ChatHandler.Create);
app.MapGet("/api/testPolling/{alias}", UpdateHandler.TestPolling);
app.MapGet("/api/update",UpdateHandler.GetUpdate);
app.Run();