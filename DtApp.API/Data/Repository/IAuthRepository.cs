using System.Threading.Tasks;
using DtApp.API.Models;

namespace DtApp.API.Data.Repository
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}