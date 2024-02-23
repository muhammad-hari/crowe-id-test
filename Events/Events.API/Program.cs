using Events.API.Extensions;
using Events.API.Helper;
using Events.Data.Context;
using Events.Domain.Models.JWT;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Pagination Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriPagination>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor!.HttpContext!.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriPagination(uri);
});


// App Services
builder.Services.AddAppService();

// Auto Mapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Data Services
builder.Services.AddDbContext<EventsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DEV")));
builder.Services.AddDataSource();

// Security Services
var jwtTokenConfiguration = builder.Configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
builder.Services.AddAppSecurity(jwtTokenConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
