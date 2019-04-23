using System.Collections.Generic;
using System.Threading.Tasks;
using DtApp.API.Models;

namespace DtApp.API.Data.Repository
{
    public interface IDtAppRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
    }
}