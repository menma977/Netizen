using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netizen.Controller
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
          if (responseData.ToString().Contains("ChanceTooHigh"))
          {
            responseData["code"] = 500;
            responseData["data"] = "Chance Too High" + jsonConverter;
            return responseData;
          }
          else if (responseData.ToString().Contains("ChanceTooLow"))
          {
            responseData["code"] = 500;
            responseData["data"] = "Chance Too Low";
            return responseData;
          }
          else if (responseData.ToString().Contains("InsufficientFunds"))
          {
            responseData["code"] = 500;
            responseData["data"] = "Insufficient Funds";
            return responseData;
          }
          else if (responseData.ToString().Contains("NoPossibleProfit"))
          {
            responseData["code"] = 500;
            responseData["data"] = "No Possible Profit";
            return responseData;
          }
          else if (responseData.ToString().Contains("MaxPayoutExceeded"))
          {
            responseData["code"] = 500;
            responseData["data"] = "Max Payout Exceeded";
            return responseData;
          }
          else if (responseData.ToString().Contains("www.999doge.com"))
          {
            responseData["code"] = 500;
            responseData["data"] = "Invalid request";
            return responseData;
          }
          else if (responseData.ToString().Contains("error"))
          {
            responseData["code"] = 500;
            responseData["data"] = "Invalid request";
            return responseData;
          }
          else if (responseData.ToString().Contains("TooFast"))
          {
            responseData["code"] = 500;
            responseData["data"] = "Too Fast" + jsonConverter;
            return responseData;
          }
          else
          {
            responseData["code"] = 200;
            responseData["data"] = jsonConverter;
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