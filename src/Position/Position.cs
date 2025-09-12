namespace Soenneker.Quark.Components.Position;

/// <summary>
/// Simplified position utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Position
{
    /// <summary>
    /// Static positioning.
    /// </summary>
    public static PositionBuilder Static => new("static");

    /// <summary>
    /// Relative positioning.
    /// </summary>
    public static PositionBuilder Relative => new("relative");

    /// <summary>
    /// Absolute positioning.
    /// </summary>
    public static PositionBuilder Absolute => new("absolute");

    /// <summary>
    /// Fixed positioning.
    /// </summary>
    public static PositionBuilder Fixed => new("fixed");

    /// <summary>
    /// Sticky positioning.
    /// </summary>
    public static PositionBuilder Sticky => new("sticky");
}
