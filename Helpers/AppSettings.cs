namespace AuthServer.Helpers
{
  public class AppSettings
  {
    public string Secret { get; set; }
    public string ExpiresInHours { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
  }
}