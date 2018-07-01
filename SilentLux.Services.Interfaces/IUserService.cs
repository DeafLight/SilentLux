using SilentLux.Model;
using System.Threading.Tasks;

namespace SilentLux.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password, out LocalUser user);
        Task<bool> AddLocalUserAsync(string id, string password, string displayName, string email);
        Task<SocialUser> AddSocialUserAsync(string id, string displayName, string email);
        Task<IUser> GetUserByIdAsync(string id);
    }
}
