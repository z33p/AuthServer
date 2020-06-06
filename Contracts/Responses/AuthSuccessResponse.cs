namespace AuthServer.Contracts.V2.Responses
{
  public class AuthSuccessResponse
  {
    public string Token { get; set; }

    public string RefreshToken { get; set; }
  }
}