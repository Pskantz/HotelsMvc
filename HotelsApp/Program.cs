using HotelApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Lägg till ApplicationDbContext i DI-container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Lägg till Identity-tjänster
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Lägg till services för Controllers och Views
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Skapa administratörsanvändare
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var adminRole = new IdentityRole("Admin");
    if (!string.IsNullOrEmpty(adminRole.Name) && !await roleManager.RoleExistsAsync(adminRole.Name))
    {
        await roleManager.CreateAsync(adminRole);
    }

    var adminEmail = "admin@exampel.com";
    var adminPassword = "Admin123!";

    var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
    var user = await userManager.FindByEmailAsync(adminEmail);
    if (user == null)
    {
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded && !string.IsNullOrEmpty(adminRole.Name))
    {
    await userManager.AddToRoleAsync(adminUser, adminRole.Name);
    }

    }
}

app.Run();
