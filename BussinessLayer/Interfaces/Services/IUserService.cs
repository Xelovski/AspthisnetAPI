using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace BussinessLayer.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByPublicIIdAsync(Guid publicId);
        Task<UserDTO?> GetByIdAsync(int id);
        Task<bool> CreateAsync(UserDTO model);
        Task<bool> UpdateAsync(UserDTO model);
        Task<bool> DeleteAsync(Guid publicId);
        Task<List<Guid>> GetAllPublicIdAsync();
        Task<bool> Register(LoginDTO model);
        Task<bool> LoginAsync(LoginDTO log);
    }
}
