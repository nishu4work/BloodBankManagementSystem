using AuthService.Models;

namespace AuthService.IRepository
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByName(string roleName);
        Task<Role> GetRoleById(int roleId);
        Task<IEnumerable<Role>> GetAllRoles();
        Task AddRole(Role role);
        Task UpdateRole(Role role);
        Task DeleteRole(int roleId);
    }
}
