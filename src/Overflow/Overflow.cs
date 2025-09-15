namespace Soenneker.Quark.Components.Overflow;

/// <summary>
/// Simplified overflow utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Overflow
{
    /// <summary>
    /// Overflow auto.
    /// </summary>
    public static OverflowBuilder Auto => new("auto");

    /// <summary>
    /// Overflow hidden.
    /// </summary>
    public static OverflowBuilder Hidden => new("hidden");

    /// <summary>
    /// Overflow visible.
    /// </summary>
    public static OverflowBuilder Visible => new("visible");

    /// <summary>
    /// Overflow scroll.
    /// </summary>
    public static OverflowBuilder Scroll => new("scroll");
}


