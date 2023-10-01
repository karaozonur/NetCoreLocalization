using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using NetCoreLocalization.Resources;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();	
builder.Services.AddSingleton<LanguageService>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(
		   options =>
		   {
			   var supportedCultures = new List<CultureInfo>
				   {
							new CultureInfo("en-US"),
							new CultureInfo("tr-TR")
				   };

			   options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
			   options.SupportedCultures = supportedCultures;
			   options.SupportedUICultures = supportedCultures;

			   options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
		   });

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

app.UseAuthorization();

var loca = app.Services.GetRequiredService <IOptions<RequestLocalizationOptions>> ();
app.UseRequestLocalization(loca.Value);


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
