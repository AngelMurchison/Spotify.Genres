using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Spotify.Genres.Models;

namespace Spotify.Genres.Services {

    public interface ITokenService {
        Task<SpotifyToken> GetClientToken (string requestToken);
    }

    public class TokenService : ITokenService {
        private IHttpClientFactory _clientFactory;
        IOptions<SpotifySettings> _spotifySettings;

        SpotifyToken token;

        public TokenService (IHttpClientFactory clientFactory, IOptions<SpotifySettings> spotifySettings) {
            _clientFactory = clientFactory;
            _spotifySettings = spotifySettings;
        }

        public async Task<SpotifyToken> GetClientToken (string requestToken) {

            return await this.GetToken ();
            // var client = _clientFactory.CreateClient ();

            // requestToken = Base64Encode (requestToken);

            // client.DefaultRequestHeaders.Clear ();
            // // There are only two snips of content that the request should contain.

            // // One in the header.
            // client.DefaultRequestHeaders.Add ("Authorization", $"Basic {requestToken}");

            // // And one in the body.
            // var values = new Dictionary<string, string> { { "grant_type", "client_credentials" } };

            // var content = new FormUrlEncodedContent (values);

            // var response = await client.PostAsync ("https://accounts.spotify.com/api/token", content);

            // var jsonString = await response.Content.ReadAsStringAsync ();

            // // The data comes back as a Json Object.
            // return JsonConvert.DeserializeObject<object> (jsonString);

        }

        private string Base64Encode (string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes (plainText);
            return System.Convert.ToBase64String (plainTextBytes);
        }

        private async Task<SpotifyToken> GetToken () {
            var token = new SpotifyToken ();

            var client = _clientFactory.CreateClient ();

            var client_id = this._spotifySettings.Value.ClientId;
            var client_secret = this._spotifySettings.Value.ClientSecret;

            var clientCreds = System.Text.Encoding.UTF8.GetBytes ($"{client_id}:{client_secret}");

            client.DefaultRequestHeaders.Clear ();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue ("Basic", System.Convert.ToBase64String (clientCreds));

            var postMessage = new Dictionary<string, string> ();
            postMessage.Add ("grant_type", "client_credentials");
            postMessage.Add ("scope", "access_token");

            var request = new HttpRequestMessage (HttpMethod.Post, this._spotifySettings.Value.TokenUrl) {
                Content = new FormUrlEncodedContent (postMessage)
            };

            var response = await client.SendAsync (request);
            if (response.IsSuccessStatusCode) {
                var json = await response.Content.ReadAsStringAsync ();
                this.token = JsonConvert.DeserializeObject<SpotifyToken> (json);
                this.token.ExpiresAt = DateTime.UtcNow.AddSeconds (this.token.ExpiresIn);
            } else {

                throw new ApplicationException ("Unable to retrieve access token from Okta");
            }

            return this.token;
        }

    }
    public class SpotifyToken {

        [JsonProperty (PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty (PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Scope { get; set; }

        [JsonProperty (PropertyName = "token_type")]
        public string TokenType { get; set; }

        public bool IsValidAndNotExpiring {
            get {
                return !String.IsNullOrEmpty (this.AccessToken) &&
                    this.ExpiresAt > DateTime.UtcNow.AddSeconds (30);
            }
        }

    }
}