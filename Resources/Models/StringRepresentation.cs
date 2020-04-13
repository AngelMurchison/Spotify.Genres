namespace Spotify.Genres.Models
{
  public class StringRepresentation : System.Attribute
  {
      private string _value;
      public StringRepresentation(string value)
      {
          _value = value;
      }
      public string Value
      {
          get { return _value; }
      }
  } 
}