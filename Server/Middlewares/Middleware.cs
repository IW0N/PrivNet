namespace Server.Middlewares
{
    public abstract class Middleware
    {
        protected RequestDelegate _next;
        public Middleware(RequestDelegate next)
        {
            
            _next=next;
        }
    }
}
