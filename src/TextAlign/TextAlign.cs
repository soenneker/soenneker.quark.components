namespace Soenneker.Quark.Components.TextAlign;

public static class TextAlign
{
    public static TextAlignBuilder Start => new("start");
    public static TextAlignBuilder Center => new("center");
    public static TextAlignBuilder End => new("end");
}
