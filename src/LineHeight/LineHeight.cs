using Soenneker.Quark.Enums.Size;
using Soenneker.Quark.Enums.Scales;

namespace Soenneker.Quark.Components.LineHeight;

public static class LineHeight
{
    public static LineHeightBuilder L1 => new(Scale.S1.Value);
    public static LineHeightBuilder Sm => new(Size.Small.Value);
    public static LineHeightBuilder Base => new("base");
    public static LineHeightBuilder Lg => new(Size.Large.Value);
}
