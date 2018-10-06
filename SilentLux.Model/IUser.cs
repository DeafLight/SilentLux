namespace SilentLux.Model
{
    public interface IUser
    {
        string Id { get; }
        string DisplayName { get; }
        EmailString Email { get; }
    }
}
