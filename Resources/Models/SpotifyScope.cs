using System;

namespace Spotify.Genres.Models
{
  [Flags]
  public enum SpotifyScope
  {
    [StringRepresentation("")]
    None = 1,

    [StringRepresentation("playlist-modify-public")]
    PlaylistModifyPublic = 2,

    [StringRepresentation("playlist-modify-private")]
    PlaylistModifyPrivate = 4,

    [StringRepresentation("playlist-read-private")]
    PlaylistReadPrivate = 8,

    [StringRepresentation("streaming")]
    Streaming = 16,

    [StringRepresentation("user-read-private")]
    UserReadPrivate = 32,

    [StringRepresentation("user-read-email")]
    UserReadEmail = 64,

    [StringRepresentation("user-library-read")]
    UserLibraryRead = 128,

    [StringRepresentation("user-library-modify")]
    UserLibraryModify = 256,

    [StringRepresentation("user-follow-modify")]
    UserFollowModify = 512,

    [StringRepresentation("user-follow-read")]
    UserFollowRead = 1024,

    [StringRepresentation("user-read-birthdate")]
    UserReadBirthdate = 2048,

    [StringRepresentation("user-top-read")]
    UserTopRead = 4096,

    [StringRepresentation("playlist-read-collaborative")]
    PlaylistReadCollaborative = 8192,

    [StringRepresentation("user-read-recently-played")]
    UserReadRecentlyPlayed = 16384,

    [StringRepresentation("user-read-playback-state")]
    UserReadPlaybackState = 32768,

    [StringRepresentation("user-modify-playback-state")]
    UserModifyPlaybackState = 65536,

    [StringRepresentation("user-read-currently-playing")]
    UserReadCurrentlyPlaying = 131072,

    [StringRepresentation("app-remote-control")]
    AppRemoteControl = 262144,

    [StringRepresentation("ugc-image-upload")]
    UgcImageUpload = 524288
  }
}