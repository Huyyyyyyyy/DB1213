using DBAdmin.Interfaces;
using DBAdmin.Services;
using DBDatabase;
using DBUtils;

var builder = WebApplication.CreateBuilder(args);

var pgConnection = Environment.GetEnvironmentVariable("Postgres")
    ?? builder.Configuration.GetConnectionString("Postgres")
    ?? throw new Exception("Postgres connection missing");

// Swagger
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddSingleton<SimpleLogger>();
builder.Services.AddSingleton(sp =>
{
    var logger = sp.GetRequiredService<SimpleLogger>();
    return new DBExecutor(pgConnection, logger);
});

// Services
builder.Services.AddScoped<IMission, MissionService>();
builder.Services.AddScoped<IUser, UserService>();

builder.Services.AddControllers();

// CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Secure", policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// Build app
var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Secure");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
