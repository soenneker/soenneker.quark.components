using Soenneker.Quark.Enums.Size;

namespace Soenneker.Quark.Components.BoxShadow;

public static class BoxShadow
{
    public static BoxShadowBuilder None => new(Soenneker.Quark.Enums.BoxShadows.BoxShadow.NoneValue);
    public static BoxShadowBuilder Base => new("base");
    public static BoxShadowBuilder Sm => new(Soenneker.Quark.Enums.Size.Size.Small.Value);
    public static BoxShadowBuilder Lg => new(Soenneker.Quark.Enums.Size.Size.Large.Value);
}
