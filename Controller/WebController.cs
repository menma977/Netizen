using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netizen.Controller
{
  public class WebController
  {
    private static readonly HttpClient Client = new HttpClient();

    public async Task<JObject> Post(Dictionary<String, String> body)
    {
      var content  = new FormUrlEncodedContent(body);
      var response = await Client.PostAsync("https://pluckywin.com/api/trade/index.php", content);
      var result   = await response.Content.ReadAsStringAsync();

      var jsonConverter = JObject.Parse(result);

      var responseData = new JObject();
      try
      {
        if (response.IsSuccessStatusCode)
        {
          if ((int) responseData["Status"] == 0)
          {
            responseData["code"] = 200;
            responseData["data"] = jsonConverter;
            return responseData;
          }
          else
          {
            responseData["code"] = 500;
            responseData["data"] = (String) jsonConverter["Pesan"];
            return responseData;
          }
        }

        responseData["code"] = 500;
        responseData["data"] = "Unstable connection / Response Not found";
        return responseData;
      }
      catch (Exception e)
      {
        responseData["code"] = 500;
        responseData["data"] = e.Message;
        return responseData;
      }
    }
  }
}