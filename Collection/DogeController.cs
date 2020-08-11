using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Serialization.Json;

namespace Netizen.Collection
{
  public class DogeController
  {
    private static readonly HttpClient Client = new HttpClient();

    public async Task<JObject> Post(Dictionary<String, String> body)
    {
      var content  = new FormUrlEncodedContent(body);
      var response = await Client.PostAsync("https://www.999doge.com/api/web.aspx", content);
      var result   = await response.Content.ReadAsStringAsync();

      var jsonConverter = JObject.Parse(result);

      var responseData = new JObject();
      try
      {
        if (response.IsSuccessStatusCode)
        {
          responseData["code"] = 200;
          responseData["data"] = jsonConverter;
          return responseData;
        }

        responseData["code"] = 500;
        responseData["data"] = "error";
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