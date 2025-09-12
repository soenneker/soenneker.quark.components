namespace Soenneker.Quark.Components.Gap;

/// <summary>
/// Simplified gap utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Gap
{
    /// <summary>
    /// No gap (0).
    /// </summary>
    public static GapBuilder S0 => new(0);

    /// <summary>
    /// Size 1 gap (0.25rem).
    /// </summary>
    public static GapBuilder S1 => new(1);

    /// <summary>
    /// Size 2 gap (0.5rem).
    /// </summary>
    public static GapBuilder S2 => new(2);

    /// <summary>
    /// Size 3 gap (1rem).
    /// </summary>
    public static GapBuilder S3 => new(3);

    /// <summary>
    /// Size 4 gap (1.5rem).
    /// </summary>
    public static GapBuilder S4 => new(4);

    /// <summary>
    /// Size 5 gap (3rem).
    /// </summary>
    public static GapBuilder S5 => new(5);
}
