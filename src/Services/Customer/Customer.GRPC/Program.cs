using Customer.GRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGrpcService<CustomerService>();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Customer gRPC service.");
});

app.Run();