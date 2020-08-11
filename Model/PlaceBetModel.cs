using System;

namespace Netizen.Model
{
  public class PlaceBetModel
  {
    public long BetId { get; set; }

    public long PayOut { get; set; }

    public string Secret { get; set; }

    public long StartingBalance { get; set; }

    public string ServerSeed { get; set; }

    public string Next { get; set; }
    
    public string Error { get; set; }
  }
}