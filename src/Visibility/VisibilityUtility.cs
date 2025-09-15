namespace Soenneker.Quark.Components.Visibility;

public static class VisibilityUtility
{
    public static VisibilityBuilder Visible => new("visible");
    public static VisibilityBuilder Invisible => new("invisible");
}
