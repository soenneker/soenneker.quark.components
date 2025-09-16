using Soenneker.Quark.Enums.Size;

namespace Soenneker.Quark.Components.LineHeight;

public static class LineHeight
{
    public static LineHeightBuilder L1 => new("1");
    public static LineHeightBuilder Sm => new(Size.Small.Value);
    public static LineHeightBuilder Base => new("base");
    public static LineHeightBuilder Lg => new(Size.Large.Value);
}
