using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.TextTransform;

public static class TextTransform
{
    public static TextTransformBuilder Lowercase => new(Soenneker.Quark.Enums.TextTransforms.TextTransform.LowercaseValue);
    public static TextTransformBuilder Uppercase => new(Soenneker.Quark.Enums.TextTransforms.TextTransform.UppercaseValue);
    public static TextTransformBuilder Capitalize => new(Soenneker.Quark.Enums.TextTransforms.TextTransform.CapitalizeValue);

    public static TextTransformBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static TextTransformBuilder Initial => new(GlobalKeyword.InitialValue);
    public static TextTransformBuilder Revert => new(GlobalKeyword.RevertValue);
    public static TextTransformBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static TextTransformBuilder Unset => new(GlobalKeyword.UnsetValue);
}
