using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TutorX.Client;
using TutorX.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with the API base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7076") });

// Register services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentGroupService, StudentGroupService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IDrawService, DrawService>();

await builder.Build().RunAsync();
