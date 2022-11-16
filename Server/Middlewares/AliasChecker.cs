using Server.Database;
using Server.Database.Base;
using Server.Database.Base.Aliases;

namespace Server.Middlewares
{
    public class AliasChecker:Middleware
    {
        static readonly Dictionary<string,string> exceptionPathes = new() 
        {
            {"/api/user","POST" }
           
        };
        public AliasChecker(RequestDelegate next):base(next)
        {
          
        }
        string CompressAliasId(string aliasId)
        {
            const int newAliasLength = 5;//first n symbols, that to be returned
            string newAlias = aliasId[..newAliasLength];
            return newAlias;
        }
        async Task CheckForAliasCorrectness(HttpContext context,PrivNetDb db,string aliasId)
        {
            UserAlias? alias = await db.UserAliases.FindAsync(aliasId);
            if (alias == null)
            {
                string compressedAlias = CompressAliasId(aliasId);
                string error = $"Alias {compressedAlias}... not found!";
                await ResponseByError(context, error, 404);
            }
            else
            {
                context.Items.Add("aliasId",aliasId);
                await _next(context);
            }
            
            
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
           
            bool isExceptionPath = exceptionPathes.ContainsKey(path) && exceptionPathes[path]==request.Method;
            
            if (isExceptionPath)
                await _next(requestContext);
            else
                await CheckAlias(request, db);
        }
    }
}
