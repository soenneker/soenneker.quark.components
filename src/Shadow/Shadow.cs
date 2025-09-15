namespace Soenneker.Quark.Components.Shadow;

public static class Shadow
{
    public static ShadowBuilder None => new("none");
    public static ShadowBuilder Base => new("base");
    public static ShadowBuilder Sm => new("sm");
    public static ShadowBuilder Lg => new("lg");
}
