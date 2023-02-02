using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Extensions;
using WebAPI.Data;
using WebAPI.IRepository;
using WebAPI.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

//var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings").GetChildren().FirstOrDefault(j => j.Key == "key")!.Value
            ));

//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("shh...this is my top secret"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = key
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// handling exception globally.
app.ConfigureCustomExceptionHandler();

//app.UseMiddleware<ExceptionMiddleware>();

//app.UseHsts();

//app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
