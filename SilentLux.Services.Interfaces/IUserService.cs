using LanguageExt;
using SilentLux.Model;
using System.Threading.Tasks;

namespace SilentLux.Services.Interfaces
{
    public interface IUserService
    {
        OptionAsync<LocalUser> ValidateCredentialsAsync(string username, string password);
        Task<bool> AddLocalUserAsync(string id, string password, string displayName, EmailString email);
        Task<SocialUser> AddSocialUserAsync(string id, string displayName, EmailString email);
        Task<IUser> GetUserByIdAsync(string id);
    }
}
