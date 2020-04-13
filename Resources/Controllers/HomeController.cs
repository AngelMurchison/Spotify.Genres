using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Spotify;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spotify.Genres.Presentation.ViewModels;
using Spotify.Genres.Resources.Models;
using SpotifyAPI.Web;

namespace Spotify.Genres.Resources.Controllers
{

  [Authorize(AuthenticationSchemes = "Spotify")]
  public class HomeController : Controller
  {
    public async Task<IActionResult> Index()
    {
      var accessToken = await HttpContext.GetTokenAsync("Spotify", "access_token");
      SpotifyWebAPI api = new SpotifyWebAPI
      {
        AccessToken = accessToken,
        TokenType = "Bearer"
      };

      var savedTracks = await api.GetSavedTracksAsync(50);

      return View(new IndexViewModel { SavedTracks = savedTracks });
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [Route("/Home/Login")]
    public IActionResult Login(string code)
    {
      return View();
    }

    public IActionResult Logout()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}