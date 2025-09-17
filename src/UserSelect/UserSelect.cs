using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.UserSelect;

public static class UserSelect
{
    public static UserSelectBuilder None => new(Soenneker.Quark.Enums.UserSelects.UserSelect.NoneValue);
    public static UserSelectBuilder Auto => new(Soenneker.Quark.Enums.UserSelects.UserSelect.AutoValue);
    public static UserSelectBuilder All => new(Soenneker.Quark.Enums.UserSelects.UserSelect.AllValue);

    public static UserSelectBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static UserSelectBuilder Initial => new(GlobalKeyword.InitialValue);
    public static UserSelectBuilder Revert => new(GlobalKeyword.RevertValue);
    public static UserSelectBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static UserSelectBuilder Unset => new(GlobalKeyword.UnsetValue);
}
