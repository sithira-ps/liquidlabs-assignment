using liquidlabs_assignment.Data;
using liquidlabs_assignment.Middleware;
using liquidlabs_assignment.Models;
using liquidlabs_assignment.Repositories;
using liquidlabs_assignment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configurations
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.Configure<ExternalApiConfig>(builder.Configuration.GetSection("ExternalApi"));
builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// custom services
builder.Services.AddScoped<ICountriesService, CountriesService>();

// repositories
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseStatusCodePages(async context =>
{
    context.HttpContext.Response.ContentType = "application/json";
    await context.HttpContext.Response.WriteAsJsonAsync(new ErrorResponse
    {
        status = "failed",
        error = $"Endpoint Not Found ({context.HttpContext.Response.StatusCode})",
        details = $"The route {context.HttpContext.Request.Path} doesn't exist"
    });
});
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
