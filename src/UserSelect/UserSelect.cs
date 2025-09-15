namespace Soenneker.Quark.Components.UserSelect;

public static class UserSelect
{
    public static UserSelectBuilder None => new("none");
    public static UserSelectBuilder Auto => new("auto");
    public static UserSelectBuilder All => new("all");
}
