using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using SuperariLife.Model.Config;
using SuperariLife.Model.Settings;
using SuperariLife.Service.Logger;
using SuperariLifeAPI;
using SuperariLifeAPI.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Logger Configuration
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
RegisterService.RegisterServices(builder.Services);

// Application Setting & SMTP Settings Configuration read from appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<DataConfig>(builder.Configuration.GetSection("Data"));
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SMTPSettings"));
builder.Services.ConfigureLoggerService();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMemoryCache();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllRequests", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("content-disposition");
    });
});
// Logger Configuration
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

// Application Setting & SMTP Settings Configuration read from appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// JWT Token Configuration
var key = Encoding.UTF8.GetBytes(Convert.ToString(builder.Configuration["AppSettings:JWT_Secret"]));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SUPERARILIFE API",
        Description = "SUPERARILIFE .NET 7 Core Web API"
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();
app.UseMiddleware<CustomMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperariLife Web Api"));

app.UseRouting();
app.UseCors("AllRequests");
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "pdf",
        pattern: "pdf/download",
        defaults: new { controller = "Agent", action = "DownloadPdf" });
    endpoints.MapControllers();
});
app.UseHttpsRedirection();

app.UseAuthorization();
string startupPath = System.IO.Directory.GetCurrentDirectory();
string ExceptionLogsPath = startupPath + "\\Logs\\ExceptionLogs";
if (!Directory.Exists(ExceptionLogsPath))
{
    System.IO.Directory.CreateDirectory(ExceptionLogsPath);
}
string RequestLogsPath = startupPath + "\\Logs\\RequestLogs";
if (!Directory.Exists(RequestLogsPath))
{
    System.IO.Directory.CreateDirectory(RequestLogsPath);
}
app.UseStaticFiles();
app.MapControllers();

app.Run();
