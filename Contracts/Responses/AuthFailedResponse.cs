using System.Collections.Generic;

namespace AuthServer.Contracts.V2.Responses
{
  public class AuthFailedResponse
  {
    public IEnumerable<string> Errors { get; set; }
  }
}