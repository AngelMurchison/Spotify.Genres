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

namespace Spotify.Genres3 {
    public class Startup {
        public Startup (IConfiguration configuration, IWebHostEnvironment environment) {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddCors (options => {
                options.AddPolicy ("AllowSpotify",
                    builder => {
                        builder.WithOrigins ("accounts.spotify.com")
                            .AllowAnyHeader ()
                            .AllowAnyMethod ();
                    });
            });

            // services.AddHttpsRedirection(options => {
            //     options.
            // })

            services.AddAuthentication (options => {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = Configuration["Spotify"];
                })
                .AddOAuth ("Spotify", options => {
                    options.ClientId = Configuration["Spotify:ClientId"];
                    options.ClientSecret = Configuration["Spotify:ClientSecret"];
                    options.CallbackPath = new PathString (Configuration["Spotify:RedirectUri"]);
                    options.Scope.Add(Configuration["Spotify:Scope"]);

                    options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
                    options.TokenEndpoint = "https://accounts.spotify.com/api/token";
                    options.UserInformationEndpoint = "https://accounts.spotify.com/authorize";

                    options.ClaimActions.MapJsonKey ("urn:spotify:login", "login");
                    options.ClaimActions.MapJsonKey ("urn:spotify:url", "html_url");
                    options.ClaimActions.MapJsonKey ("urn:spotify:avatar", "avatar_url");

                    options.ClaimActions.MapJsonKey("client_id", "id");
                    
                    options.Events = new OAuthEvents {
                        OnCreatingTicket = async context => {

                            var token = new SpotifyToken ();

                            var postMessage = new Dictionary<string, string> ();
                            postMessage.Add ("grant_type", "authorization_code");
                            postMessage.Add ("code", context.TokenResponse.AccessToken);
                            postMessage.Add ("redirect_uri", new PathString (Configuration["Spotify:RedirectUri"]));
                            // postMessage.Add ("client_id", context.Options.ClientId);
                            // postMessage.Add ("client_secret", context.Options.ClientSecret);

                            var request = new HttpRequestMessage (HttpMethod.Post, context.Options.TokenEndpoint) {
                                Content = new FormUrlEncodedContent (postMessage)
                            };

                            // request.Headers.Clear();
                            var authCode = Convert.ToBase64String(Encoding.UTF8.GetBytes ($"{context.Options.ClientId}:{context.Options.ClientSecret}"));
                            
                            request.Headers.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/x-www-form-urlencoded"));
                            request.Headers.Authorization = new AuthenticationHeaderValue ("Basic", authCode);

                            // var request = new HttpRequestMessage (HttpMethod.Get, context.Options.TokenEndpoint);
                            // request.Headers.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
                            // request.Headers.Authorization = new AuthenticationHeaderValue ("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync (request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            // response.EnsureSuccessStatusCode ();

                            // var jsonuser = new JsonElement ();

                            // var user = JsonConvert.DeserializeAnonymousType (await response.Content.ReadAsStringAsync (), jsonuser);

                            // context.RunClaimActions (user);
                        }
                    };
                });

            services.AddDataProtection ();

            services.Configure<SpotifySettings> (Configuration.GetSection ("Spotify"));

            services.AddHttpClient ();

            services.AddScoped<ITokenService, TokenService> ();

            services.Configure<CookiePolicyOptions> (options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.HttpOnly = HttpOnlyPolicy.None;
                options.Secure = _environment.IsDevelopment () ?
                    CookieSecurePolicy.None : CookieSecurePolicy.Always;
            });

            services.AddControllersWithViews ()
                .AddNewtonsoftJson ();
            services.AddRazorPages ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseCors ("AllowSpotify");

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseCookiePolicy ();

            app.UseAuthentication ();

            app.UseEndpoints (endpoints => {

                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute (
                    name: "privacy",
                    pattern: "{controller=Home}/{action=Privacy}/{id?}");

                endpoints.MapRazorPages ();
            });
        }
    }
}