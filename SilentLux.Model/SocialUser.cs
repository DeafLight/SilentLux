using System;

namespace SilentLux.Model
{
    public class SocialUser : IUser
    {
        private SocialUser()
        {
        }

        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public EmailString Email { get; private set; }

        public static SocialUser Create(string id, string displayName, EmailString email)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (displayName == null) throw new ArgumentNullException(nameof(displayName));
            if (email == null) throw new ArgumentNullException(nameof(email));

            return new SocialUser
            {
                Id = id,
                DisplayName = displayName,
                Email = email
            };
        }
    }
}
