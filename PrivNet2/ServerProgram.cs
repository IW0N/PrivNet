using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.RequestHandlers;
using Server.Database.Entities;
using Server.Services;
using Common.Services;

var builder = WebApplication.CreateBuilder(args);
var configs = builder.Configuration;
string connectionStr=configs.GetConnectionString("privNetDb");
builder.Services.AddDbContext<PrivNetDb>(
    options=>options.UseSqlServer(connectionStr)
    );
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddSingleton<TokenGenerator>();
var app = builder.Build();
app.MapPost("/signUp",SignupHandler.SignUp);
//app.MapGet("/api/user/chats",);
app.Run();