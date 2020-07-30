using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incohearent.Data
{
    // Interface - rad s tablicom korisnika
    public interface IUserStore
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUser(int id);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(User user);       
    }
}
