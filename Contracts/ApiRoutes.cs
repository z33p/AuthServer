namespace AuthServer.Contracts.V2
{
  public static class ApiRoutes
  {
    //   public const string Root = "api";

    public const string Version = "v2";

    //   public const string Base = Root + "/" + Version;

    public static class Auth
    {
      public const string Register = Version + "/register";
      public const string Login = Version + "/login";
      public const string Logout = Version + "/logout";
      public const string Refresh = Version + "/refresh";
      public const string User = Version + "/user";
      public const string Users = Version + "/users";
    }
  }
}