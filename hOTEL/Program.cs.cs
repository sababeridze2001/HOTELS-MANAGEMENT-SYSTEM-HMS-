using Hotel.Service;
using Hotel.Repository;
using Microsoft.EntityFrameworkCore;
using Hotel.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7273); // 7273
    options.ListenAnyIP(7274, listenOptions => listenOptions.UseHttps()); // 7274
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});


builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddSingleton<IGuestService, GuestService>(); 
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IUserService, UserService>();


var jwtSettings = builder.Configuration.GetSection("Jwt"); 


var secretKey = jwtSettings["Key"]; 
if (string.IsNullOrEmpty(secretKey))
{
    throw new ArgumentNullException("JWT SecretKey is missing in the configuration.");
}

var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new ArgumentNullException("JWT Issuer or Audience is missing in the configuration.");
}

var key = Encoding.UTF8.GetBytes(secretKey); 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = issuer,
            ValidAudience = audience
        };
    });

var app = builder.Build();

app.UseHttpsRedirection(); 

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();
app.Run();
