namespace AuthServer.Contracts.V2.Requests
{
  public class RefreshTokenRequest
  {
    public string Token { get; set; }
    public string RefreshToken { get; set; }
  }
}