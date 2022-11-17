using Server.RequestHandlers;
using Server;
using Server.Middlewares.Extensions;
using static Server.Services.NoRequiredRegisterPaths;
var builder = WebApplication.CreateBuilder(args);
builder.Configure();
var app = builder.Build();

app.MapPost("/api/user", SignupHandler.SignUp);
app.MapWhen(context => PathNotRequireRegister(context), builder =>
{
    builder.UseAliasChecker();
    builder.UseUserExtractor();
    builder.UseRequestDecryptor();

    builder.UseMapPost("/api/user/chat", ChatHandler.Create);
    builder.UseMapGet("/api/user/update", UpdateHandler.GetUpdate);
    builder.UseMapDelete("/api/user/update", UpdateHandler.DeleteUpdate);
    builder.UseMapGet("/api/users", UserHandler.FindUsers);
    builder.UseMapGet("/api/user",UserHandler.FindUser);

    builder.UseTemporaryUpdater();
    builder.RunEncryption();
});
app.Run();