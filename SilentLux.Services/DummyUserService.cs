using SilentLux.Model;
using SilentLux.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilentLux.Services
{
    public class DummyUserService : IUserService
    {
        private IDictionary<string, (string PasswordHash, User User)> _users =
            new Dictionary<string, (string PasswordHash, User User)>(StringComparer.OrdinalIgnoreCase);

        public DummyUserService(IDictionary<string, string> users)
        {
            foreach (var user in users)
            {
                _users.Add(user.Key, (BCrypt.Net.BCrypt.HashPassword(user.Value), new User(user.Key)));
            }
        }

        public Task<bool> ValidateCredentials(string username, string password, out User user)
        {
            user = null;

            var key = username;

            if (_users.ContainsKey(key))
            {
                var hash = _users[key].PasswordHash;

                if (BCrypt.Net.BCrypt.Verify(password, hash))
                {
                    user = _users[key].User;

                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public Task<bool> AddUser(string username, string password)
        {
            if (_users.ContainsKey(username))
            {
                Task.FromResult(false);
            }

            _users.Add(username, (BCrypt.Net.BCrypt.HashPassword(password), new User(username)));

            return Task.FromResult(true);
        }
    }
}
