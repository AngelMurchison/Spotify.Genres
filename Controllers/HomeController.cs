using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spotify.Genres3.Models;

namespace Spotify.Genres3.Controllers {
    public class HomeController : Controller {
        public IActionResult Index () {
            return Challenge (new AuthenticationProperties (), "Spotify");
        }

        public IActionResult Privacy () {
            return View ();
        }

        public async void Login () {
            var claims = new List<Claim> {
                new Claim (ClaimTypes.Name, Guid.NewGuid ().ToString ())
            };

            var claimsIdentity = new ClaimsIdentity (
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties ();

            await HttpContext.SignInAsync (
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal (claimsIdentity),
                authProperties);
        }

        public IActionResult Logout () {
            return View ();
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}