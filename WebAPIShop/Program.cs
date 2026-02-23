using Microsoft.EntityFrameworkCore;
using Repository;
using Servers;
using Entitys;
using NLog.Web;
using Services;
using WebAPIShop;
using WebAPIShop.Middleware;
using Microsoft.AspNetCore.Builder;
using PresidentsApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IPrudectsService, PrudectsService>();


builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();


builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddDbContext<dbSHOPContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("projectAPI")));

builder.Host.UseNLog();

builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API V1");
    });
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseErrorHandlingMiddleware();

app.UseRatingMiddleware();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
