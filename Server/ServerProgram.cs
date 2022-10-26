using Server.RequestHandlers;
using Server;
using Server.Middlewares.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();
var app = builder.Build();
app.UseAliasChecker();
app.MapPost("/api/user",SignupHandler.SignUp);
app.MapPost("/api/user/chat",ChatHandler.Create);
app.MapGet("/api/user/update",UpdateHandler.GetUpdate);
app.MapDelete("/api/user/update",UpdateHandler.DeleteUpdate);
app.Run();