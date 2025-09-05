using Restaurants.Domain.Exceptions;

namespace Restaurants.Api.Middlewares
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> _logger): IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);

            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (ForbidException ex)
            {
                _logger.LogWarning(ex.Message);
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something goes wrong");
            }
        }
    }
}
