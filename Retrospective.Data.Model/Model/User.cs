using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
  public class User
  {
    public ObjectId? Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public bool IsDemoUser { get; set; }
    public DateTime? SubscriptionEnd { get; set; }

    public DateTime? LastLoggedIn { get; set; }

    public string AuthenticationSource { get; set; }

    public string AuthenticationID { get; set; }


  }
}
