using liquidlabs_assignment.Models;
using liquidlabs_assignment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configurations
builder.Services.Configure<ExternalApiConfig>(builder.Configuration.GetSection("ExternalApi"));
builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});
// custom services
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
