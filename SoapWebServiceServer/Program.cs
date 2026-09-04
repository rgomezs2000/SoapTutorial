using SoapWebServiceServer.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ AddControllers() UNA SOLA VEZ
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Permitir lectura múltiple del body
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.MapControllers();

app.Run();