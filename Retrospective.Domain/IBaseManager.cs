using DomainModel = Retrospective.Domain.Model;

namespace Retrospective.Domain
{
  public interface IBaseManager
  {

    DomainModel.User GetUser(string userId);
  }
}