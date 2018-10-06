using System;

namespace SilentLux.Model
{
    public class LocalUser : IUser
    {
        private LocalUser()
        {
        }

        public string Id { get; private set; }
        public string PasswordHash { get; private set; }
        public string DisplayName { get; private set; }
        public EmailString Email { get; private set; }

        public static LocalUser Create(string id, string passwordHash, string displayName, EmailString email)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (passwordHash == null) throw new ArgumentNullException(passwordHash);
            if (displayName == null) throw new ArgumentNullException(nameof(displayName));
            if (email == null) throw new ArgumentNullException(nameof(email));

            return new LocalUser
            {
                Id = id,
                PasswordHash = passwordHash,
                DisplayName = displayName,
                Email = email
            };
        }
    }
}
