﻿using Server.Database;
using Server.Database.Base.Aliases;

namespace Server.Middlewares
{
    public class AliasChecker
    {
        private readonly RequestDelegate next;
        static readonly Dictionary<string,string> exceptionPathes = new() 
        {
            {"/api/user","POST" }
        };
        public AliasChecker(RequestDelegate next)
        {
            this.next = next;
        }
        async Task CheckForAliasCorrectness(HttpContext context,PrivNetDb db,string aliasId)
        {
            UserAlias? alias = await db.UserAliases.FindAsync(aliasId);
            if (alias == null)
            {
                string error = $"Alias {aliasId} not found!";
                await ResponseByError(context, error, 404);
            }
            else
                await next(context);
        }
        static async Task ResponseByError(HttpContext context,string errorMessage,int statusCode)
        {
            HttpResponse response = context.Response;
            response.StatusCode = statusCode;
            await response.WriteAsync(errorMessage);
        }
        async Task CheckAlias(HttpRequest request,PrivNetDb db)
        {
            string aliasId = request.Query["aliasId"];
            var context = request.HttpContext;
            if (string.IsNullOrWhiteSpace(aliasId))
            {
                string error = $"You can not use '{request.Path}' without user alias!";
                //401 is status code (unauthorized)
                await ResponseByError(context, error, 401);
            }
            else
                await CheckForAliasCorrectness(context, db, aliasId);
        }
        public async Task InvokeAsync(HttpContext requestContext, PrivNetDb db)
        {
            HttpRequest request = requestContext.Request;
            string path = request.Path;
           
            bool isExpetionPath = exceptionPathes.ContainsKey(path) && exceptionPathes[path]==request.Method;
            if (isExpetionPath)
                await next(requestContext);
            else
                await CheckAlias(request, db);
        }
    }
}