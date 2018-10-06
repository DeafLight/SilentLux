using LanguageExt;
using SilentLux.Model;
using SilentLux.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace SilentLux.Services
{
    public class DummyUserService : IUserService
    {
        private readonly IDictionary<string, IUser> _users =
            new Dictionary<string, IUser>(StringComparer.OrdinalIgnoreCase);

        public DummyUserService(IDictionary<string, (string Password, string DisplayName, EmailString Email)> users)
        {
            foreach (var user in users)
            {
                _users.Add(user.Key, LocalUser.Create(user.Key, BCrypt.Net.BCrypt.HashPassword(user.Value.Password), user.Value.DisplayName, user.Value.Email));
            }
        }

        public OptionAsync<LocalUser> ValidateCredentialsAsync(string username, string password)
        {
            var key = username;

            if (_users.ContainsKey(key))
            {
                var localUser = _users[key] as LocalUser;

                var hash = localUser.PasswordHash;

                if (BCrypt.Net.BCrypt.Verify(password, hash))
                {
                    return OptionalAsync(Task.FromResult(localUser));
                }
            }

            return None;
        }

        public Task<bool> AddLocalUserAsync(string username, string password, string displayName, EmailString email)
        {
            if (_users.ContainsKey(username))
            {
                Task.FromResult(false);
            }

            _users.Add(username, LocalUser.Create(username, BCrypt.Net.BCrypt.HashPassword(password), displayName, email));

            return Task.FromResult(true);
        }

        public Task<SocialUser> AddSocialUserAsync(string id, string displayName, EmailString email)
        {
            var user = SocialUser.Create(id, displayName, email);

            _users.Add(id, user);

            return Task.FromResult(user);
        }

        public Task<IUser> GetUserByIdAsync(string id)
        {
            if (_users.ContainsKey(id))
            {
                return Task.FromResult(_users[id]);
            }

            return Task.FromResult<IUser>(null);
        }
    }
}
