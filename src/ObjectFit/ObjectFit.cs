namespace Soenneker.Quark.Components.ObjectFit;

/// <summary>
/// Simplified object-fit utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class ObjectFit
{
    /// <summary>
    /// object-fit: contain.
    /// </summary>
    public static ObjectFitBuilder Contain => new("contain");

    /// <summary>
    /// object-fit: cover.
    /// </summary>
    public static ObjectFitBuilder Cover => new("cover");

    /// <summary>
    /// object-fit: fill.
    /// </summary>
    public static ObjectFitBuilder Fill => new("fill");

    /// <summary>
    /// object-fit: scale-down.
    /// </summary>
    public static ObjectFitBuilder ScaleDown => new("scale-down");

    /// <summary>
    /// object-fit: none.
    /// </summary>
    public static ObjectFitBuilder None => new("none");
}


