using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApplication;
using WebApplication.Service.DishManagement;
using WebApplication.Service.Identity;
using WebApplication.Service.UserManagement;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IdentityManager>();
builder.Services.AddScoped<IdentityHttpClient>();

builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<DishService>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
