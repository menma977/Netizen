using System;
using System.Collections.Generic;
using System.Linq;

namespace Netizen.Config
{
  public class MapToJson
  {
    public String map(Dictionary<String, String> map)
    {
      String body = "";
      int    lang = map.Count;
      int    i    = 1;
      foreach (var item in map)
      {
        if (lang == i)
        {
          body = body + item.Key + "=" + item.Value;
        }
        else
        {
          body = body + item.Key + "=" + item.Value + "&";
        }

        i++;
      }

      return body;
    }
  }
}