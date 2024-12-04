using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();

var app = builder.Build();

var configuration = app.Services.GetService<IConfiguration>();
var hosting = app.Services.GetService<IWebHostEnvironment>();

var secrets = configuration.GetSection("Secrets").Get<AppSecrets>();
DbInitializer.appSecrets = secrets;



using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("Starting seeding!");
    DbInitializer.SeedUsersAndRoles(scope.ServiceProvider).Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{ OnPrepareResponse = ctx => ctx.Context.Response.Headers.Add("X-Content-Type-Options", "nosniff") });

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Type-Options", "SAMEORIGIN");
    context.Response.Headers.Add("X-Xss-Protection", "1");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; frame-ancestors 'none'; form-action 'self'");
    await next();
});

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
