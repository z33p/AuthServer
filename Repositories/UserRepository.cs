using System.Linq;
using System.Collections.Generic;
using AuthServer.Models;

namespace AuthServer.Repositories
{
  public static class UserRepository
  {
    public static User Get(string username, string password)
    {
      var users = new List<User>();
      users.Add(new User { Id = 0, Username = "batman", Password = "batman", Role = "manager" });
      users.Add(new User { Id = 1, Username = "robin", Password = "robin", Role = "employee" });
      return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefault();
    }
  }
}
