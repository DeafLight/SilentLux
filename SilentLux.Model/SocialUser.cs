namespace SilentLux.Model
{
    public class SocialUser : IUser
    {
        private SocialUser()
        {
        }

        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public string Email { get; private set; }

        public static SocialUser Create(string id, string displayName, string email)
        {
            return new SocialUser
            {
                Id = id,
                DisplayName = displayName,
                Email = email
            };
        }
    }
}
