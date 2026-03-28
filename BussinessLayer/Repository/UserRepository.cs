using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Datalayer;
using WebApplication2.Datalayer.Entities;

namespace BussinessLayer.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context):base(context) {
        
            _context = context;
        }

        public async Task<UserEntity?> GetEmailAsync(string emil)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Email == emil);
        }
    }
}
