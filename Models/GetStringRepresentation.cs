using System;
using System.Reflection;

namespace Spotify.Genres3.Models
{
  public static class GetStringRepresentationExtension
  {
    public static string GetStringRepresentation(this SpotifyScope value)
    {
      var output = "";

      Type type = value.GetType();

      var enums = value.ToString().Split(", ");

      foreach (var str in enums)
      {
        FieldInfo fi = type.GetField(str);

        StringRepresentation[] attrs = fi.GetCustomAttributes(typeof(StringRepresentation), false) as StringRepresentation[];

        if (attrs.Length > 0)
        {
          if (output == "")
          {
            output = attrs[0].Value;
          }
          else
          {
            output = String.Join(", ", output, attrs[0].Value);
          }
        }
      }

      return output;
    }
  }
}