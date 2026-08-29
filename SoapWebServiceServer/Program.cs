using SoapWebServiceServer.Services;
using CoreWCF;
using CoreWCF.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddServiceModelServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder
        .AddService<ProductService>()
        .AddServiceEndpoint<ProductService, IProductService>(
            new BasicHttpBinding(),
            "");  // ✅ Ruta vacía
});

app.MapControllers();

app.Run();