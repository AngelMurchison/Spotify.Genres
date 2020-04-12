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
using AspNet.Security.OAuth.Spotify;
using Spotify.Genres3.Models;

namespace Spotify.Genres3.Controllers {
    public class HomeController : Controller 
    {

      [Route("")]
        public IActionResult Index () 
        {
            return Challenge ("Spotify");
        }

        public IActionResult Privacy () 
        {
            return View ();
        }
        [Route("/Home/Login")]
        public IActionResult Login (string code) 
        {
            return View();
        }

        public IActionResult Logout () 
        {
            return View ();
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () 
        {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}