namespace Soenneker.Quark.Components.TextTransform;

public static class TextTransform
{
    public static TextTransformBuilder Lowercase => new("lowercase");
    public static TextTransformBuilder Uppercase => new("uppercase");
    public static TextTransformBuilder Capitalize => new("capitalize");
}
