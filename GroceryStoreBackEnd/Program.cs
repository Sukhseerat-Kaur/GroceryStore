using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using GroceryStore.DataLayer.Context;
using GroceryStore.DataLayer.UnitOfWork;
using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStore.DataLayer.Repository;
using GroceryStore.DataLayer.Repository.Interfaces;
using GroceryStoreBackEnd.Utilities.Configuration;
using GroceryStoreCore.Utilities.Configuration.Interfaces;
using GroceryStoreDomain.Services;
using GroceryStoreDomain.Services.Interfaces;
using GroceryStoreFacade.FacadeInterfaces;
using GroceryStoreFacade.Facade;
using GroceryStoreCore.Filters;
using FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation;
using GroceryStoreBackEnd.Validators;
using GroceryStoreBackEnd.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc(options =>
{
    options.Filters.Add<GroceryStoreExceptionFilterAttribute>();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});
// Validators (fluent Validation)
builder.Services.AddScoped<GroceryStoreExceptionFilterAttribute>();
builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<UserViewModel>, UserValidator>();
builder.Services.AddScoped<IValidator<UserLoginViewModel>, UserLoginValidator>();
builder.Services.AddScoped<IValidator<ProductCreationViewModel>, ProductValidator>();
builder.Services.AddScoped<IValidator<OrderViewModel>, OrderValidator>();
builder.Services.AddScoped<IValidator<CartViewModel>,  CartValidator>();
builder.Services.AddScoped<IValidator<List<OrderViewModel>>, UserOrderValidator>();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddDbContext<GroceryStoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("GroceryStoreConnectionString")));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuers = new[] { "https://localhost:7123", "http://localhost:5289", "https://localhost:5001" },
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});
builder.Services.AddAuthorization();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IImageService,ImageService>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// App Configuration
builder.Services.AddScoped<IAppConfiguration, AppConfiguration>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// Unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Facade
builder.Services.AddScoped<IFacade, Facade>();

builder.Services.AddScoped<DbSeeder>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<DbSeeder>();
    seeder.SeedSuperAdminUser();
    seeder.SeedRoles();
    seeder.SeedCategories();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseForwardedHeaders();


app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
