using System;

namespace app.Model
{
  public class User
  {
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsDemoUser { get; set; }


    /// <summary>
    /// indicates the source of the user authentiction
    /// ex; Google
    /// </summary>
    /// <value></value>
    public string AuthenticationSource { get; set; }

    public string AuthenticationID { get; set; }

    public DateTime? LastLoggedIn { get; set;}
  }
}