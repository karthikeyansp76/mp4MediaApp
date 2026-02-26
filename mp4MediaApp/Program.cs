using mp4MediaApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Scoped services are created once per client request (connection).
//This is ideal for services that should be tied to the lifecycle of a single request,
//such as database contexts or services that manage user sessions. By registering IVideoService as scoped,
//we ensure that each HTTP request gets its own instance of VideoService, which can help manage resources effectively
//and maintain state specific to that request if needed.

builder.Services.AddScoped<IVideoService, VideoService>();   // Register the video service for dependency injection 


var app = builder.Build();

// Added to Ensure if media folder does not exist

var mediaPath = Path.Combine(app.Environment.WebRootPath, "media");

if (!Directory.Exists(mediaPath))
{
    Directory.CreateDirectory(mediaPath);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<mp4MediaApp.Middleware.GlobalExceptionMiddleware>(); // custom global exception handling middleware

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
