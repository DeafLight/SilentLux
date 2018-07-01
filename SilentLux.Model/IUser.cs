namespace SilentLux.Model
{
    public interface IUser
    {
        string Id { get; }
        string DisplayName { get; }
        string Email { get; }
    }
}
