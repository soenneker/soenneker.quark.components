namespace Soenneker.Quark.Components.TextDecoration;

public static class TextDecoration
{
    public static TextDecorationBuilder None => new("none");
    public static TextDecorationBuilder Underline => new("underline");
    public static TextDecorationBuilder LineThrough => new("line-through");
}
