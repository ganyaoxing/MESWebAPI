using MESWebAPI.Models;

namespace MESWebAPI.Service;
#nullable disable
public interface IAuthenticateService
{
    string Authenticated(User mesUser);
}
