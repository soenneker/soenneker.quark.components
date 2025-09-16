using Soenneker.Quark.Enums.Size;

namespace Soenneker.Quark.Components.BoxShadow;

public static class BoxShadow
{
    public static BoxShadowBuilder None => new("none");
    public static BoxShadowBuilder Base => new("base");
    public static BoxShadowBuilder Sm => new(Size.Small.Value);
    public static BoxShadowBuilder Lg => new(Size.Large.Value);
}
