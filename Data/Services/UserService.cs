using CashLad.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashLad.Data.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> GetByEmailAsync(string email);
    }

    public class UserService : BaseService<User>, IUserService
    {
        public UserService(DatabaseContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _repository.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }
    }
}
