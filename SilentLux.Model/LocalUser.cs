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
        public string Email { get; private set; }

        public static LocalUser Create(string id, string passwordHash, string displayName, string email)
        {
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
