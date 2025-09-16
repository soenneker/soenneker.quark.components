using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.TextWrap;

public static class TextWrap
{
    public static TextWrapBuilder Wrap => new("wrap");
    public static TextWrapBuilder NoWrap => new("nowrap");

    public static TextWrapBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static TextWrapBuilder Initial => new(GlobalKeyword.InitialValue);
    public static TextWrapBuilder Revert => new(GlobalKeyword.RevertValue);
    public static TextWrapBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static TextWrapBuilder Unset => new(GlobalKeyword.UnsetValue);
}
