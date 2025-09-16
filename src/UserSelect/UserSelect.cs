using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.UserSelect;

public static class UserSelect
{
    public static UserSelectBuilder None => new("none");
    public static UserSelectBuilder Auto => new("auto");
    public static UserSelectBuilder All => new("all");

    public static UserSelectBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static UserSelectBuilder Initial => new(GlobalKeyword.InitialValue);
    public static UserSelectBuilder Revert => new(GlobalKeyword.RevertValue);
    public static UserSelectBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static UserSelectBuilder Unset => new(GlobalKeyword.UnsetValue);
}
