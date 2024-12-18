var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. 
    // You may want to change this for production scenarios.
    app.UseHsts();
}

// Redirect HTTP to HTTPS (can be disabled if unnecessary)
app.UseHttpsRedirection();

// Enable serving static files (e.g., CSS, JavaScript, images)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map the default route for MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
