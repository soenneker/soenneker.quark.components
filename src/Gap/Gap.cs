namespace Soenneker.Quark.Components.Gap;

/// <summary>
/// Simplified gap utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Gap
{
    /// <summary>
    /// No gap (0).
    /// </summary>
    public static GapBuilder S0 => new(Soenneker.Quark.Enums.Scales.Scale.S0.Value);

    /// <summary>
    /// Size 1 gap (0.25rem).
    /// </summary>
    public static GapBuilder S1 => new(Soenneker.Quark.Enums.Scales.Scale.S1.Value);

    /// <summary>
    /// Size 2 gap (0.5rem).
    /// </summary>
    public static GapBuilder S2 => new(Soenneker.Quark.Enums.Scales.Scale.S2.Value);

    /// <summary>
    /// Size 3 gap (1rem).
    /// </summary>
    public static GapBuilder S3 => new(Soenneker.Quark.Enums.Scales.Scale.S3.Value);

    /// <summary>
    /// Size 4 gap (1.5rem).
    /// </summary>
    public static GapBuilder S4 => new(Soenneker.Quark.Enums.Scales.Scale.S4.Value);

    /// <summary>
    /// Size 5 gap (3rem).
    /// </summary>
    public static GapBuilder S5 => new(Soenneker.Quark.Enums.Scales.Scale.S5.Value);
}
