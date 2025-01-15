using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonthlyDataApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IMonthlyDataService, MonthlyDataService>();
builder.Services.AddScoped<LoginService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidIssuer = "https://shirat-moshe-server.onrender.com",  // הכנס את ה-issuer שלך
//            ValidAudience = "https://shirat-moshe.onrender.com",  // הכנס את ה-audience שלך
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-long-secret-key-32-bytes-long!your-very-long-secret-key-32-bytes-long!")) // הכנס את המפתח שלך
//        };
//    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        Console.WriteLine("JWT Authentication is configured");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = Environment.GetEnvironmentVariable("https://shirat-moshe-server.onrender.com"),
            ValidAudience = Environment.GetEnvironmentVariable("https://shirat-moshe.onrender.com"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-long-secret-key-32-bytes-long!"))
        };
    });



// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("https://shirat-moshe.onrender.com", "http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
            Console.WriteLine("CORS policy set for: https://shirat-moshe.onrender.com, http://localhost:4200");

        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Make sure to use CORS before UseAuthorization or UseEndpoints
app.UseCors("AllowSpecificOrigins");
Console.WriteLine("CORS middleware is applied");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

