using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spotify.Genres.Business.Services;
using Spotify.Genres.Resources.Models;

namespace Spotify.Genres.Resources.Controllers
{

  [ApiController]
  public class AuthController : Controller
  {
    private ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    [Route("spotify/token")]
    [HttpGet]
    [EnableCors("AllowSpotify")]
    public IActionResult GetSpotifyClientToken(string tokenId = "f28cc0ca6f1e4faea933e93c1f389749:26ac4ea4b1dd413a8321cc0d32fd7249")
    {

      var result = _tokenService.GetClientToken(tokenId);

      return Json(result);
    }
  }
}