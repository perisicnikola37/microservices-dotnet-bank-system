var builder = WebApplication.CreateBuilder(args);

// Dodavanje potrebnih servisa
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHealthChecksUI(setup =>
{
    setup.AddCustomStylesheet("style.css");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();