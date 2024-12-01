//using MonthlyDataApi.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddScoped<IMonthlyDataService, MonthlyDataService>();

//// Configure CORS
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

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();



//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

////app.UseHttpsRedirection();


//// Make sure to use CORS before UseAuthorization or UseEndpoints
//app.UseCors("AllowAllOrigins");

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using MonthlyDataApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IMonthlyDataService, MonthlyDataService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
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


//app.UseHttpsRedirection();

// Make sure to use CORS before UseAuthorization or UseEndpoints
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

// Serve static files (this is where your Angular app will be served)
app.UseStaticFiles(); // This serves files from the wwwroot folder

// Serve the Angular app's index.html for any non-API routes
app.MapFallbackToFile("index.html"); // This is to handle Angular routing

app.MapControllers();

app.Run();
