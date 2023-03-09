using Microsoft.EntityFrameworkCore;
using ZamaraWebApp.Data;
using Microsoft.AspNetCore.Identity;
using ZamaraWebApp.Models;

var builder = WebApplication.CreateBuilder(args);
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// Add services to the container.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("WebPolicy", policy => policy.RequireClaim("Staff"));
    options.AddPolicy("WebPolicy", policy => policy.RequireClaim("posts"));
    options.AddPolicy("WebPolicy", policy => policy.RequireClaim("Continents"));
    options.AddPolicy("WebPolicy", policy => policy.RequireClaim("Reports"));
    //options.AddPolicy("WebPolicy", policy => policy.RequireClaim("Staff"));
});
var connectionString = builder.Configuration.GetConnectionString("defaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<UserViewModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
