using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Spotify.Genres3.Models;
using Spotify.Genres3.Services;

namespace Spotify.Genres3
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      _environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment _environment { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services
        .AddAuthentication(o => o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie()
        .AddSpotify(options =>
        {
          var scopes = SpotifyScope.UserLibraryRead | SpotifyScope.UserModifyPlaybackState;
          options.Scope.Add(scopes.GetStringRepresentation());

          options.SaveTokens = true;
          options.ClientId = Configuration["Spotify:ClientId"];
          options.ClientSecret = Configuration["Spotify:ClientSecret"];
          options.CallbackPath = new PathString(Configuration["Spotify:RedirectUri"]);

          options.Events.OnAccessDenied = async context => 
          {
            return;
          };

          options.Events.OnCreatingTicket = async context => 
          {
            return;
          };

          options.Events.OnRemoteFailure = async context => 
          {
            return;
          };

          options.Events.OnTicketReceived = async context => 
          {
            // It comes here and then redirects back to the challenge endpoint, which is the root.
            return;
          };


        });

      services.AddHttpClient();

      services.AddScoped<ITokenService, TokenService>();

      services.AddControllersWithViews()
        .AddNewtonsoftJson();
      services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller}/{action}/{id?}");

        endpoints.MapRazorPages();
      });
    }
  }
}