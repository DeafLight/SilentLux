using SilentLux.Model;
using System.Threading.Tasks;

namespace SilentLux.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password, out User user);
        Task<bool> AddUser(string username, string password);
    }
}
