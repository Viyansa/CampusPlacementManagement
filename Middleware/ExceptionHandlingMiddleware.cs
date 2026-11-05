using System.Text.Json;
namespace CampusPlacementManagement.Middleware;
public class ExceptionHandlingMiddleware
{
 private readonly RequestDelegate _next;private readonly ILogger<ExceptionHandlingMiddleware> _logger;public ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger){_next=next;_logger=logger;}
 public async Task InvokeAsync(HttpContext context){try{await _next(context);}catch(Exception ex){_logger.LogError(ex,"Unhandled exception");context.Response.ContentType="application/problem+json";context.Response.StatusCode=ex switch{UnauthorizedAccessException=>StatusCodes.Status401Unauthorized,KeyNotFoundException=>StatusCodes.Status404NotFound,InvalidOperationException=>StatusCodes.Status400BadRequest,_=>StatusCodes.Status500InternalServerError};var body=new{type="https://httpstatuses.com/"+context.Response.StatusCode,title=context.Response.StatusCode==500?"An unexpected error occurred":ex.Message,status=context.Response.StatusCode,traceId=context.TraceIdentifier};await context.Response.WriteAsync(JsonSerializer.Serialize(body));}}
}
