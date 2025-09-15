namespace Soenneker.Quark.Components.TextWrap;

public static class TextWrap
{
    public static TextWrapBuilder Wrap => new("wrap");
    public static TextWrapBuilder NoWrap => new("nowrap");
}
