using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.Size;

namespace Soenneker.Quark.Components.Utilities;

/// <summary>
/// Shared utilities for converting breakpoints to CSS class tokens.
/// </summary>
public static class BreakpointUtilities
{
    /// <summary>
    /// Converts a breakpoint to its corresponding CSS class token.
    /// Returns empty string for phone/extra-small (default) breakpoints.
    /// </summary>
    /// <param name="breakpoint">The breakpoint to convert</param>
    /// <returns>The CSS class token (e.g., "sm", "md", "lg", "xl", "xxl") or empty string</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetBreakpointToken(Breakpoint? breakpoint)
    {
        if (breakpoint is null)
            return string.Empty;

        switch (breakpoint)
        {
            case Breakpoint.PhoneValue:
            case Breakpoint.ExtraSmallValue:
                return string.Empty;
            case Breakpoint.MobileValue:
            case Breakpoint.SmallValue:
                return Size.Small.Value;
            case Breakpoint.TabletValue:
            case Breakpoint.MediumValue:
                return Size.Medium.Value;
            case Breakpoint.LaptopValue:
            case Breakpoint.LargeValue:
                return Size.Large.Value;
            case Breakpoint.DesktopValue:
            case Breakpoint.ExtraLargeValue:
                return Size.ExtraLarge.Value;
            case Breakpoint.ExtraExtraLargeValue:
                return Size.ExtraExtraLarge.Value;
            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// Converts a breakpoint to its corresponding CSS class token.
    /// Alias for GetBreakpointToken for backward compatibility.
    /// </summary>
    /// <param name="breakpoint">The breakpoint to convert</param>
    /// <returns>The CSS class token (e.g., "sm", "md", "lg", "xl", "xxl") or empty string</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetBreakpointClass(Breakpoint? breakpoint) => GetBreakpointToken(breakpoint);
}
