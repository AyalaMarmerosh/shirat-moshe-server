using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonthlyDataApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddSingleton<AuthService>();

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
//            ValidIssuer = "https://shirat-moshe-server.onrender.com",  // ???? ?? ?-issuer ???
//            ValidAudience = "https://shirat-moshe.onrender.com",  // ???? ?? ?-audience ???
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-long-secret-key-32-bytes-long!your-very-long-secret-key-32-bytes-long!")) // ???? ?? ????? ???
//        };
//    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"Challenge: {context.Error}");
                return Task.CompletedTask;
            },
        };
        Console.WriteLine("JWT Authentication is configured");
        var config = builder.Configuration.GetSection("Jwt");
        var validIssuer = config["Issuer"];
        var validAudience = config["Audience"];
        //Console.WriteLine($"Issuer: {validIssuers[0]}, {validIssuers[1]}");
        //Console.WriteLine($"Audience: {validAudiences[0]}, {validAudiences[1]}");
        //Console.WriteLine($"Key: {config["Key"]}");
        //Console.WriteLine($"Signing Key Length: {config["Key"].Length}");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Key"]))
        };
    });



// Configure CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://shirat-moshe.onrender.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
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

//app.Use(async (context, next) =>
//{
//    // הוספת כותרת CSP
//    context.Response.Headers.Add("Content-Security-Policy",
//        "connect-src 'self' https://shirat-moshe-server.onrender.com;");

//    // המשך לטפל בשאר הבקשה
//    await next();
//});

// Make sure to use CORS before UseAuthorization or UseEndpoints
app.UseCors("AllowSpecificOrigins");
Console.WriteLine("CORS middleware is applied");


app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        context.Response.StatusCode = 401; // Unauthorized
        await context.Response.WriteAsync("Unauthorized: " + ex.Message);
    }
});

app.MapControllers();

app.Run();