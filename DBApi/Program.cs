using DBApi.Services;
using DBDatabase;
using DBUtils;
using Microsoft.Extensions.FileProviders;

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
builder.Services.AddScoped<GeneralService>();
builder.Services.AddControllers();

// CORS (optional)
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
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
app.UseCors("Open");

if (!Directory.Exists(Const.ROOT_MEDIA_DIRECTORY)) Directory.CreateDirectory(Const.ROOT_MEDIA_DIRECTORY);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Const.ROOT_MEDIA_DIRECTORY),
    RequestPath = "/uploads"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
